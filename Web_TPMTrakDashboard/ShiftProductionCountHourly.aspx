<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftProductionCountHourly.aspx.cs" Inherits="Web_TPMTrakDashboard.ShiftProductionCountHourly" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter {
            margin-bottom: 10px;
        }

        #container0, #container1, #container2,  #container3, #container4, #container5 {
            margin-left: -14px;
        }

        .tblkwh td {
            /*width:20%;
        height:45px;*/
        }

        .tbl td {
            width: 20%;
            /*height:48px;*/
            /*padding-top:16px;*/
        }

        .col-lg-1, col-lg-6, col-lg-3, .row {
            text-align: center;
        }

        .col1, col-lg-6 {
            height: 80px;
            padding-top: 18px;
            border: 1px solid white;
        }

        .cl6 {
            height: 80px;
            padding-top: 29px;
            border: 0.5px solid white;
        }

        .cl2 {
            height: 80px;
            border: 0.5px solid white;
        }

        .cl66 {
            height: 40px;
            border: 1px solid white;
            border-width: 0.5px 0 0 1px;
            padding-top: 8px;
        }

        .pad {
            padding-top: 8px;
        }

        .val {
            border: 0.5px solid white;
            width: 20%;
            height: 38px;
        }

        .kwh {
            border: 0.5px solid white;
        }

        .kwhcol {
            border: 0.5px solid white;
        }

        .tbl tr {
            transition: background 0.2s ease-in;
        }

            .tbl tr:nth-child(odd) {
                background-color: whitesmoke;
            }

            .tbl tr:hover {
                background-color: chartreuse;
                cursor: pointer;
            }
    </style>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/VDGjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>

    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>

            <div class="row text-center">
                <asp:HiddenField ID="hdfValue" runat="server" />
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                <table class="table table-bordered" id="tblfilter" style="width: 60%">
                    <tr>
                        <td><b>Plant ID</b></td>
                        <td>
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td><b>Machine ID</b></td>
                        <td>
                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Machine ID !"></asp:DropDownList></td>
                        <td><b>Date</b></td>
                        <td class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                title="Date !" placeholder="Date"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" Text="Process" class="btn btn-info btn-sm" ID="btnProcess" OnClick="btnProcess_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="row text-center" style="font-size: 20px; font-weight: 900;">
                <asp:Label ID="Label1" runat="server" Text="Hourly Tracking - 18681 STUDER"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row" style="padding: 0 0 2px 0;">
        <asp:UpdatePanel ID="updatePanel1" runat="server">
            <ContentTemplate>
                <div class="row" style="margin: 0; color: white; background-color: #2E6886; border-radius: 8px;">
                    <div class="col-lg-1 col1" style="border: 0.5px solid white">
                        <div class="row"></div>
                        <div class="row  pad"><b>Time</b></div>
                    </div>
                    <div class="col-lg-2 cl2">
                        <div class="row" style="height: 40px;">
                            <b>Target</b>
                            <br />
                            (Based on 100% OEE)
                        </div>
                        <div class="row">
                            <div class="col-lg-6 cl66"><b>Pcs</b></div>
                            <div class="col-lg-6 cl66"><b>Cumu</b></div>
                        </div>
                    </div>
                    <div class="col-lg-2 cl2">
                        <div class="row" style="height: 40px; padding-top: 10px;">
                            <b>Actual</b>
                        </div>
                        <div class="row" style="height: auto">
                            <div class="col-lg-6 cl66"><b>Pcs</b></div>
                            <div class="col-lg-6 cl66"><b>Cumu</b></div>
                        </div>
                    </div>
                    <div class="col-lg-4 cl1" style="padding-top: 10px;">
                        <b>Pieces / hour</b>
                    </div>
                    <div class="col-lg-3 col1" style="padding-top: 29px">
                        <b>KWH</b>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

  	<div class="row" style="margin: 0 2px 6px 2px;">
			<asp:UpdatePanel ID="updatePanel2" runat="server">
				<ContentTemplate>
					<div class="col-lg-3">
						<div class="row">
							<table class="table table-bordered tbl">
								<tr >
									<td style="font-size: 8px;width:18px" class="time"><b>
										<label id="lblshift1hour1" runat="server" style="font-weight: 400;" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour1TPCS" meta:resourcekey="lbl67TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour1TCumu" meta:resourcekey="lbl67TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour1APCS" meta:resourcekey="lbl67APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour1ACumu" meta:resourcekey="lbl67ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour2" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour2TPCS" meta:resourcekey="lbl78TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour2TCumu" meta:resourcekey="lbl78TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour2APCS" meta:resourcekey="lbl78APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour2ACumu" meta:resourcekey="lbl78ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour3" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour3TPCS" meta:resourcekey="lbl89TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour3TCumu" meta:resourcekey="lbl89TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour3APCS" meta:resourcekey="lbl89APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour3ACumu" meta:resourcekey="lbl89ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour4" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour4TPCS" meta:resourcekey="lbl910TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour4TCumu" meta:resourcekey="lbl910TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour4APCS" meta:resourcekey="lbl910APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour4ACumu" meta:resourcekey="lbl910ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour5" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour5TPCS" meta:resourcekey="lbl1011TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour5TCumu" meta:resourcekey="lbl1011TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour5APCS" meta:resourcekey="lbl1011APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour5ACumu" meta:resourcekey="lbl1011ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour6" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour6TPCS" meta:resourcekey="lbl1112TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour6TCumu" meta:resourcekey="lbl1112TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour6APCS" meta:resourcekey="lbl1112APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour6ACumu" meta:resourcekey="lbl1112ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour7" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour7TPCS" meta:resourcekey="lbl1213TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour7TCumu" meta:resourcekey="lbl1213TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour7APCS" meta:resourcekey="lbl1213APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour7ACumu" meta:resourcekey="lbl1213ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour8" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour8TPCS" meta:resourcekey="lbl1314TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour8TCumu" meta:resourcekey="lbl1314TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour8APCS" meta:resourcekey="lbl1314APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour8ACumu" meta:resourcekey="lbl1314ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift1hour9" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour9" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour9TPCS" meta:resourcekey="lbl1415TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour9TCumu" meta:resourcekey="lbl1415TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour9APCS" meta:resourcekey="lbl1415APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour9ACumu" meta:resourcekey="lbl1415ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift1hour10" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour10" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour10TPCS" meta:resourcekey="lbl1415TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour10TCumu" meta:resourcekey="lbl1415TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour10APCS" meta:resourcekey="lbl1415APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour10ACumu" meta:resourcekey="lbl1415ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift1hour11" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour11" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour11TPCS" meta:resourcekey="lbl1415TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour11TCumu" meta:resourcekey="lbl1415TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour11APCS" meta:resourcekey="lbl1415APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour11ACumu" meta:resourcekey="lbl1415ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift1hour12" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift1hour12" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour12TPCS" meta:resourcekey="lbl1415TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour12TCumu" meta:resourcekey="lbl1415TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour12APCS" meta:resourcekey="lbl1415APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift1hour12ACumu" meta:resourcekey="lbl1415ACumuResource1"></asp:Label></td>
								</tr>
								<tr style="background-color: yellow">
									<td style="font-size: 8px"><b>&Sigma; 1.S.</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl1SsumTarget" meta:resourcekey="lbl1SsumTargetResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl1SsumActual" meta:resourcekey="lbl1SsumActualResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour1" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour1TPCS" meta:resourcekey="lbl1516TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour1TCumu" meta:resourcekey="lbl1516TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour1APCS" meta:resourcekey="lbl1516APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour1ACumu" meta:resourcekey="lbl1516ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour2" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour2TPCS" meta:resourcekey="lbl1617TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour2TCumu" meta:resourcekey="lbl1617TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour2APCS" meta:resourcekey="lbl1617APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour2ACumu" meta:resourcekey="lbl1617ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour3" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour3TPCS" meta:resourcekey="lbl1718TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour3TCumu" meta:resourcekey="lbl1718TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour3APCS" meta:resourcekey="lbl1718APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour3ACumu" meta:resourcekey="lbl1718ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour4" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour4TPCS" meta:resourcekey="lbl1819TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour4TCumu" meta:resourcekey="lbl1819TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour4APCS" meta:resourcekey="lbl1819APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour4ACumu" meta:resourcekey="lbl1819ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour5" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour5TPCS" meta:resourcekey="lbl1920TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour5TCumu" meta:resourcekey="lbl1920TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour5APCS" meta:resourcekey="lbl1920APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour5ACumu" meta:resourcekey="lbl1920ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour6" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour6TPCS" meta:resourcekey="lbl2021TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour6TCumu" meta:resourcekey="lbl2021TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour6APCS" meta:resourcekey="lbl2021APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour6ACumu" meta:resourcekey="lbl2021ACumuResource1"></asp:Label></td>
								</tr>
								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour7" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour7TPCS" meta:resourcekey="lbl2122TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour7TCumu" meta:resourcekey="lbl2122TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour7APCS" meta:resourcekey="lbl2122APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour7ACumu" meta:resourcekey="lbl2122ACumuResource1"></asp:Label></td>
								</tr>

								<tr>
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour8" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour8TPCS" meta:resourcekey="lbl2223TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour8TCumu" meta:resourcekey="lbl2223TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour8APCS" meta:resourcekey="lbl2223APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour8ACumu" meta:resourcekey="lbl2223ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift2hour9" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour9" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour9TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour9TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour9APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour9ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift2hour10" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour10" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour10TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour10TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour10APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour10ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift2hour11" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour11" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour11TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour11TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour11APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour11ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift2hour12" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift2hour12" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour12TPCS" meta:resourcekey="lbl2324TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour12TCumu" meta:resourcekey="lbl2324TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour12APCS" meta:resourcekey="lbl2324APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift2hour12ACumu" meta:resourcekey="lbl2324ACumuResource1"></asp:Label></td>
								</tr>
								<tr style="background-color: yellow">
									<td style="font-size: 8px"><b>&Sigma; 2.S.</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl2SsumTarget" meta:resourcekey="lbl2SsumTargetResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl2SsumActual" meta:resourcekey="lbl2SsumActualResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
								</tr>

								<tr id="trshift3hour1" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour1" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour1TPCS" meta:resourcekey="lbl01TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour1TCumu" meta:resourcekey="lbl01TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour1APCS" meta:resourcekey="lbl01APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour1ACumu" meta:resourcekey="lbl01ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour2" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour2" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour2TPCS" meta:resourcekey="lbl12TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour2TCumu" meta:resourcekey="lbl12TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour2APCS" meta:resourcekey="lbl12APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour2ACumu" meta:resourcekey="lbl12ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour3" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour3" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour3TPCS" meta:resourcekey="lbl23TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour3TCumu" meta:resourcekey="lbl23TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour3APCS" meta:resourcekey="lbl23APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour3ACumu" meta:resourcekey="lbl23ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour4" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour4" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour4TPCS" meta:resourcekey="lbl34TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour4TCumu" meta:resourcekey="lbl34TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour4APCS" meta:resourcekey="lbl34APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour4ACumu" meta:resourcekey="lbl34ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour5" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour5" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour5TPCS" meta:resourcekey="lbl45TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour5TCumu" meta:resourcekey="lbl45TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour5APCS" meta:resourcekey="lbl45APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour5ACumu" meta:resourcekey="lbl45ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour6" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour6" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour6TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour6TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour6APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour6ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour7" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour7" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour7TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour7TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour7APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour7ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour8" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour8" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour8TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour8TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour8APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour8ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
								</tr>
								<tr id="trshift3hour9" runat="server">
									<td style="font-size: 8px"><b>
										<label id="lblshift3hour9" runat="server" />
									</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour9TPCS" meta:resourcekey="lbl56TPCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour9TCumu" meta:resourcekey="lbl56TCumuResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour9APCS" meta:resourcekey="lbl56APCSResource1"></asp:Label></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lblshift3hour9ACumu" meta:resourcekey="lbl56ACumuResource1"></asp:Label></td>
								</tr>
								<tr style="background-color: yellow" id="trsummation3shift" runat="server">
									<td style="font-size: 8px"><b>&Sigma; 3.S.</b></td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl3SsumTarget" meta:resourcekey="lbl3SsumTargetResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
									<td style="font-size: 8px">
										<asp:Label runat="server" ID="lbl3SsumActual" meta:resourcekey="lbl3SsumActualResource1"></asp:Label></td>
									<td style="font-size: 8px">&nbsp;</td>
								</tr>
								<tr style="color: white; background-color: #2E6886;">
									<td colspan="2" style="color: white;"><b><%=GetLocalResourceObject("Total Per Day") %></b></td>
									<td>
										<asp:Label runat="server" ID="lblTotalTarget" Style="color: white" meta:resourcekey="lblTotalTargetResource1"></asp:Label></td>
									<td>&nbsp;</td>
									<td>
										<asp:Label runat="server" ID="lblTotalActual" Style="color: white" meta:resourcekey="lblTotalActualResource1"></asp:Label></td>
								</tr>
							</table>
						</div>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>

			<div class="col-lg-3" style="padding-right: 0;">
				<table>
					<tr>

					</tr>
					<tr>

					</tr>
					<tr>

					</tr>
				</table>
				<div id="container0" style="height: 426px; background-color: white; margin-right: -3px"></div>
				<div id="container1" style="height: 361px; background-color: white; margin-right: -3px"></div>
				<div id="container2" style="height: 337px; background-color: white; margin-right: -3px"></div>
				<%--<div id="container0" style="height: 467px; background-color: white;"></div>
                <div id="container1" style="height: 407px; background-color: white;"></div>
                <div id="container2" style="height: 390px; background-color: white;"></div>--%>
			</div>


			<%-- <div class="col-lg-2" style="width: 13%">

                <div id="container3" style="height: 330px; background-color: white;"></div>
                <div id="container4" style="height: 335px; background-color: white;"></div>
                <div id="container5" style="height: 345px; background-color: white;"></div>
            </div>--%>

			<%--   <asp:UpdatePanel ID="updatePanel3" runat="server">
                <ContentTemplate>--%>
			<%-- <div class="col-lg-1 cl2" style="width: 6%">
                    </div>
                    <div class="col-lg-1 cl2" style="width: 10%">
                        <div class="row" style="height: 40px; padding-top: 10px;">
                        </div>
                        <div class="row" style="height: auto">
                            <div class="col-lg-6 cl66"><b></b></div>
                            <div class="col-lg-6 cl66"><b></b></div>
                        </div>
                    </div>--%>
			<%--                    <div class="col-lg-5 cl2" style="width: 28.85%">
                        <div class="row" style="height: auto">
                        </div>
                    </div>--%>


			<%--          </ContentTemplate>
            </asp:UpdatePanel>--%>
		</div>
    </div>



    <script type="text/javascript">

        $(document).ready(function () {
            $.unblockUI({});
            BindGraph();
            $(".date").datepicker({
                format: 'dd-M-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top"
            });
        });

        function BindGraph() {
            if ($("[id$=hdfValue]").val() == "OK") {
                GetFirstShiftData();
                //GetSecondShiftData();
                //GetThirdShiftData();
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            BindGraph();

            $(".date").datepicker({
                format: 'dd-M-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top"
            });
        })

        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtDate]").val() == "") {
                alert("Please enter date !!")
                $("[id$=txtDate]").focus();
                return false;
            }
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        function GetFirstShiftData() {
            $.ajax({
                type: "POST",
                url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetFirst,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }
        //function GetSecondShiftData() {
        //    $.ajax({
        //        type: "POST",
        //        url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
        //        contentType: "application/json; charset=utf-8",
        //        data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
        //        dataType: "json",
        //        success: OnSuccessGetSecond,
        //        error: function (response) {
        //            console.log(response.d);
        //        }
        //    });
        //}
        //function GetThirdShiftData() {
        //    $.ajax({
        //        type: "POST",
        //        url: "ShiftProductionCountHourly.aspx/GetFirstShiftGraph",
        //        contentType: "application/json; charset=utf-8",
        //        data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startDate:"' + $("[id$=txtDate]").val() + '"}',
        //        dataType: "json",
        //        success: OnSuccessGetThird,
        //        error: function (response) {
        //            console.log(response.d);
        //        }
        //    });
        //}
        function OnSuccessGetFirst(Result) {
            drawFirstShiftChart(Result.d);
            drawSecondShiftChart(Result.d);
            drawThirdShiftChart(Result.d);
            KWHFirstShiftChart(Result.d);
            KWHSecondShiftChart(Result.d);
            KWHThirdShiftChart(Result.d)
        }

        function drawFirstShiftChart(data1) {
            $('#container0').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        formatter: function () {
                            return '<span style="fill: white;">' + this.value + '</span>';
                        }
                    },
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        // pointWidth: 38,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    labels: {
                    	enabled: true,
                    },
                    dataLabels: {
                    	enabled: true,
                    	allowOverlap: true
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    //barWidth: 4,
                    //  maxPointWidth: 40,
                    //  data: [1, 2, 8, 5, 2, 6, 7, 5],                
                    data: data1.TargetFirstShift,

                }, {
                    //  data: [0, 2, 3, 9, 6, 3, 7, 8, 6, 0],
                    data: data1.ActualFirstShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]
            });

        }
        //-------Second Chart-----              
        function drawSecondShiftChart(data1) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container1').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: -60,
                    // marginBottom:0,              
                    marginLeft: 0,
                    paddingLeft: 0,
                    //  columnWidth: 23,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },

                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    line: {
                        marker: {
                            enabled: false
                        }
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    // data: [1, 3, 8, 9, 1, 9, 5, 5],
                    data: data1.TargetSecondShift,

                }, {
                    //data: [0, 2, 5, 8, 4, 3, 8, 6, 1, 0],
                    data: data1.ActualSecondShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]

            });

        }
        //------Third Chart-------
        function drawThirdShiftChart(data1) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container2').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    marginTop: -58,
                    // marginBottom:0,              
                    marginLeft: 0,
                    paddingLeft: 0,
                    //  columnWidth: 23,
                    backgroundColor: 'transparent',
                    style: {
                        overflow: 'visible',
                    },

                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    line: {
                        marker: {
                            enabled: false
                        }
                    }
                    //bar: {
                    //    borderColor: '#FF555F',
                    //    borderWidth: 2,
                    //    color: '#FFFFFF',
                    //},
                },
                series: [{
                    name: 'Target',
                    type: 'bar',
                    color: '#C8D5EE',
                    //data: [1, 3, 3, 8, 2, 9, 2, 5],
                    data: data1.TargetThirdShift,

                }, {
                    // data: [0, 7, 8, 6, 4, 5, 9, 6, 1, 0],
                    data: data1.ActualThirdShift,
                    //  pointPadding: 0.4,
                    pointPlacement: 0.5,
                    lineWidth: 3,
                    color: 'green',
                    step: 'right',
                    name: 'Actual'
                }]
            });
        }

        //--------KWH Chart ----------------
        function KWHFirstShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container3').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHFirstShift,

                }]
            });
        }

        function KWHSecondShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container4').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHSecondShift,

                }]
            });
        }

        function KWHThirdShiftChart(data) {
            //  alert(data1[0].ActualFirstShift);
            // alert(data1[0].TargetFirstShift);
            $('#container5').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'bar',
                    marginTop: -4,
                    marginLeft: 0,
                    paddingLeft: 0,
                    backgroundColor: 'transparent',
                    //style: {
                    //    overflow: 'visible',
                    //},
                    legend: {
                        enabled: false
                    },
                },
                title: {
                    text: '',
                    margin: 60,
                },
                xAxis: {
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    },
                    opposite: true,

                    labels: {
                        enabled: true,
                    },
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal',
                        pointPadding: 0,
                        groupPadding: 0,
                        showInLegend: false,
                        value: 0,
                        width: 1,
                        states: {
                            hover: {
                                enabled: false
                            }
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    color: '#C8D5EE',
                    data: data.KWHThirdShift,

                }]
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
