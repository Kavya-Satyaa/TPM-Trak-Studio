<%@ Page Title="Alarm History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlarmHistory.aspx.cs" Inherits="Web_TPMTrakDashboard.Alarm_History" Culture="auto" UICulture="auto" %>
<%--meta:resourcekey="PageResource1"--%> 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
        <script src="Scripts/PDF/pdf.js"></script>
    <script src="Scripts/PDF/pdf.worker.js"></script>
    <style>
        #GridViewAlaramHistory tr th {
            position: sticky;
            top: -1px;
            background-color: #2E6886;
        }
    </style>
    <div style="width: auto; margin: 15px; max-height: 40vh">
        <table class="table table-bordered" style="width: auto; height: auto">
            <tr>
                <td style="align-content: center">
                    <asp:Label runat="server" Text="<%$ Resources:CommanResource,FromDate %>" ForeColor="White" meta:resourcekey="LabelResource1" />
                </td>
                <td class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="DD-MMM-YYYY" Style="padding-left: 3px" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled" Width="130px"></asp:TextBox>
                </td>
                  <td style="align-content: center">
                    <asp:Label runat="server" Text="<%$ Resources:CommanResource,ToDate %>" ForeColor="White" meta:resourcekey="LabelResource3" />
                </td>
                <td class="input-group" style="align-content: center">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" placeholder="DD-MMM-YYYY" Style="padding-left: 3px" meta:resourcekey="txtToDateResource1" AutoCompleteType="Disabled" Width="130px"></asp:TextBox>
                </td>
                <td style="align-content: center">
                    <asp:Label runat="server" Text="<%$ Resources:CommanResource,PlantID %>" ForeColor="White" meta:resourcekey="LabelResource2" />
                </td>
                <td style="align-content: center">
                    <asp:DropDownList runat="server" ID="ddlplant" Width="170px" Height="30px" meta:resourcekey="ddlplantResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" />
                </td>
                  <td style="align-content: center">
                    <asp:Label runat="server" Text="<%$ Resources:CommanResource,Machine %>" ForeColor="White" meta:resourcekey="LabelResource4" />
                </td>
                <td style="align-content: center">
                    <asp:DropDownList runat="server" ID="ddlMachine" Width="170px" Height="30px" meta:resourcekey="ddlMachineResource1" />
                </td>
                <td rowspan="2" style="border: none">
                    <asp:RadioButtonList ID="radiobtn" runat="server" ForeColor="White" Font-Overline="False" BackColor="White" meta:resourcekey="radiobtnResource1">
                        <asp:ListItem Text="<%$Resources:CommanResource,Summary %>" Value="Summary" Selected="True" meta:resourcekey="ListItemResource1" />
                        <asp:ListItem Text="<%$Resources:CommanResource,Details %>" Value="Details" meta:resourcekey="ListItemResource2" />
                    </asp:RadioButtonList>
                </td>
               
                <td rowspan="2" style="align-items: baseline;">

                    <asp:Button ID="btnview" runat="server" Text="<%$ Resources:CommanResource,View %>" CssClass="btn btn-primary" OnClick="view_Click" meta:resourcekey="btnSaveColumnSettingResource1" OnClientClick="return callLoader();" />
                    <asp:Button ID="btnexport" runat="server" Text="<%$ Resources:CommanResource,Export %>" CssClass="btn btn-primary" OnClick="Export_Click" meta:resourcekey="btnSaveColumnSettingResource2" />
                </td>
            </tr>
            <tr>
              
               
                 <td style="align-content: center">
                    <asp:Label runat="server" Text="<%$ Resources:CommanResource,Shift %>" ForeColor="White" meta:resourcekey="LabelResource5" />
                </td>
                <td style="align-content: center">
                    <asp:DropDownList runat="server" ID="ddlshift" Width="170px" Height="30px" meta:resourcekey="ddlshiftResource1" />
                </td>
                 <td style="align-content: center">
                    <asp:Label runat="server" Text="Alarm Group" ForeColor="White" meta:resourcekey="LabelResource6" />
                </td>
                <td style="align-content: center">
                    <asp:DropDownList runat="server" ID="ddlalarmgrp" Width="170px" Height="30px" meta:resourcekey="ddlalarmgrpResource1" />
                </td>
                   <td style="align-content: center">
                    <asp:Label runat="server" Text="Filter By" ForeColor="White" meta:resourcekey="LabelResource6" />
                </td>
                <td style="align-content: center">
                    <asp:DropDownList runat="server" ID="ddlFilterBy" Width="170px" Height="30px">
                        <asp:ListItem Value="" Text="All"></asp:ListItem>
                           <asp:ListItem Value="Lubrication" Text="Lubrication"></asp:ListItem>
                           <asp:ListItem Value="Coolant" Text="Coolant"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                  <td colspan="2" style="align-content: center">
                    <label>
                        <span class="checkbox commanTd">
                            <asp:CheckBox ID="chkAutoBox" runat="server" type="checkbox" meta:resourcekey="chkAutoBoxResource1" /><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></span>
                    </label>
                </td>
            </tr>
         <%--   <tr>
               
             
              
            </tr>--%>
        </table>
    </div>
    <div class="row text-center" style="padding: 0 0 2px 0; font-size: 20px; font-weight: 900; color: white">
        <%-- <asp:Label ID="Label1" runat="server" Text="Alarm History" meta:resourcekey="Label1Resource1"></asp:Label>--%>
        <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>
        <asp:Button ID="btnTrigger" runat="server" Style="display: none;" OnClick="btnTrigger_Click" meta:resourcekey="btnTriggerResource1" />
    </div>
    <div id="datagriddiv" style="overflow-x: auto; overflow-y: auto; margin: 05px; min-width: 100px;">
         <asp:GridView ID="GridViewAlaramHistory" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered " meta:resourcekey="grdViewAndonViewResource1" HeaderStyle-VerticalAlign="Middle" HorizontalAlign="Center" ShowHeaderWhenEmpty="True" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="GridViewAlaramHistory_RowDataBound">

            <%--<AlternatingRowStyle BackColor="White" Wrap="False" />--%>
            <Columns>
                <asp:BoundField HeaderText="SL No." DataField="SLNO" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource1">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Machine ID" HeaderStyle-Width="100" DataField="MachineID" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource2" ItemStyle-CssClass="machine">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" Width="100px"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Alarm No." DataField="AlarmNo" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource3" ItemStyle-CssClass="alarmno">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Message" DataField="Message" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource4">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="First Occurence" DataField="FirstOccurence" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource5">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Last Occurence" DataField="LastOccurence" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource6">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="No. Of Occurence" DataField="NoOfOccur" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource7">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="StartTime" DataField="StartTime" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource8">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Endtime" DataField="Endtime" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource9">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Duration" DataField="duration" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="BoundFieldResource10">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White"></HeaderStyle>
                </asp:BoundField>

            </Columns>

            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" />
            <HeaderStyle BackColor="#2E6886" Font-Bold="True" />
            <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>

    <div class="modal infoModal bajaj-info-modal" id="showDrawingModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog modal-dialog-centered" style="width: 50%;">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Drawing</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                </div>
                <div class="modal-body">
                    <div style="height: 70vh">
                        <%--<iframe id="iframeDocument" style="width: 100%; height: 100%"></iframe>--%>
                        <div id="pdfConatiner" style="height: 100%; width: 100%; overflow: auto; background-color: #f1f3f6; text-align: center; padding-top: 5px">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="button" value="Close" class="bajaj-btn-style" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var width = $(window).width();
        var height = $(window).height();

        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MMM-YYYY',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MMM-YYYY',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $("[id$=txtFromDate]").on("dp.change", function (e) {
            $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        });
        $("[id$=txtToDate]").on("dp.change", function (e) {
            $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
        });
		<%-- var from = $("[id$=txtFromDate]").val();
            var to =  $('[id$=txtToDate]').val();

            var dateCom =  compareDates(from,to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
            }--%>
        $(document).ready(function () {
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 350);
                console.log('min');
            } else {
                winHeight = (winHeight - 300);
                console.log('max');
                $("#datagriddiv").height(winHeight);
            }
            $("[id$=chkAutoBox]").click(function () {
                $("[id$=btnTrigger]").trigger("click");
                // return false;
            });
            callUnLoader();
        })
        $("[id$=chkAutoBox]").click(function () {
            $("[id$=btnTrigger]").trigger("click");
            //return false;
        });
        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        function AlarmNoClick(element) {
            let machine = $(element).closest("tr").find(".machine").text();
            if (machine == null || machine == "" || machine == undefined) {
                machine = $("[id$=ddlMachine]").val();
            }
            let alarmno = $(element).text();
            showAlarmDocuments(machine, alarmno);
        }


        let pdfPageNumber;
        function showAlarmDocuments(machine, alarmno) {
            let pdfDetails;
            $("#pdfConatiner").empty();
            $.ajax({
                async: false,
                type: "POST",
                url: "AlarmHistory.aspx/GetAlarmNoPDFDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{machine:'" + machine + "', alarmNo: '" + alarmno + "'}",
                success: function (response) {
                    pdfDetails = response.d;
                },
                error: function (response) {
                    alert("Error");
                }
            });
            if (pdfDetails.PageStartNo == "" || pdfDetails.PageEndNo == "" || pdfDetails.PageStartNo == null || pdfDetails.PageEndNo == null) {
                openWarningModal("PDF page number not mentioned.");
                return;
            }
            if (pdfDetails.PDFPath == "" || pdfDetails.PDFPath == null) {
                openWarningModal("PDF file is not there. Please mention PDF file path.");
                return;
            }

            let PDFPath = pdfDetails.PDFPath;
            let pdfStartPoint = pdfDetails.PageStartNo;
            let pdfEndPoint = pdfDetails.PageEndNo;
            var noOfPages = parseInt(pdfEndPoint) - parseInt(pdfStartPoint);
            if (noOfPages < 0) {
                openWarningModal("PDF page number not entred properly.");
                return;
            }
            pdfPageNumber = parseInt(pdfStartPoint);

            PDFJS.disableRange = true;
            PDFJS.getDocument(PDFPath).promise.then(function (pdfFDoc_) {
                PDFFileDoc = pdfFDoc_;
                while (pdfPageNumber <= pdfEndPoint) {
                    var canvas = document.createElement("canvas");
                    $("#pdfConatiner").append(canvas);
                    createPDFPages(canvas);
                    if ($("#pdfConatiner canvas").length == (noOfPages + 1)) {
                        break;
                    }
                    pdfPageNumber++;
                }
            });
            $("#showDrawingModal").modal('show');
        }
        function createPDFPages(canvas) {
            PDFFileDoc.getPage(pdfPageNumber).then(function (page) {
                viewport = page.getViewport(1);
                canvas.height = viewport.height;
                canvas.width = viewport.width;
                page.render({ canvasContext: canvas.getContext('2d'), viewport: viewport });
            });
        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
