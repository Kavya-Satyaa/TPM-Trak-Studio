<%@ Page Title="Map Rule to Machine" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MapRulesToMachines.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.MapRulesToMachines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
      <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>

    <style>
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
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }
        .headerFixerTable tr td{
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }


        .table .lbl {
            padding-top: 15px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }
        input[type="checkbox"]{
            height: 20px;
            width: 20px;
            margin-right: 5px;
            vertical-align: sub;
        }
        #gvRuleMachine tr th label{
            font-weight: bold;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>
            <div class="ui segment">
                <span style="color: white; font-weight: bold;color:black;width:180px">Select Rule</span>
                <asp:ListBox ID="ddlRules" runat="server" SelectionMode="Multiple" ToolTip="Rules" Width="150" OnSelectedIndexChanged="ddlRules_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static"></asp:ListBox>
                <asp:Button runat="server" Text="Save" class="ui violet button" ID="btnSave" OnClick="btnSave_Click"></asp:Button>
            </div>
            <div id="gridContainer" class="divGrid">
                <asp:GridView runat="server" ID="gvRuleMachine" AutoGenerateColumns="false" ClientIDMode="Static"
                    CssClass="table table-bordered cockpit headerFixerTable " ShowHeaderWhenEmpty="true" OnRowDataBound="gvRuleMachine_RowDataBound">
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=ddlRules]').multiselect({
                includeSelectAllOption: true
            });
            var Height = $(window).height() - (150);
            $('#gridContainer').css('height', Height);
            setCheckBox();
        });
        $(window).resize(function () {
            var Height = $(window).height() - (150);
            $('#gridContainer').css('height', Height);
        });
        //$('[id$=ddlRules] input[type="checkbox"]').click(function () {
        //    debugger;
        //    alert();
        //});

        $('#gvRuleMachine tr th input[type="checkbox"]').click(function () {
            debugger;
            var tblRow = $('#gvRuleMachine tr');
            var thindex = $(this).closest('th').index();
            if ($(this).prop("checked")) {
                for (var i = 1; i < tblRow.length ; i++) {
                    var td = tblRow[i].children[thindex];
                    $(td).find('input').prop("checked", true);
                }
            }
            else {
                for (var i = 1; i < tblRow.length ; i++) {
                    var td = tblRow[i].children[thindex];
                    $(td).find('input').prop("checked", false);
                }
            }
        });
        $('#gvRuleMachine tr td input[type="checkbox"]').click(function () {
            debugger;
            let tblRow = $('#gvRuleMachine tr');
            let thindex = $(this).closest('td').index();
            let count = 0;
            for (var i = 1; i < tblRow.length; i++) {
                var td = tblRow[i].children[thindex];
                if ($(td).find('input').prop("checked")) {
                    console.log("count =" + count);
                    count++;
                }
            }
              console.log("out count =" + count);
            if (count == tblRow.length - 1) {
                let header = $('#gvRuleMachine tr th')[thindex];
                $(header).find('input').prop("checked", true);
            }
            else {
                let header = $('#gvRuleMachine tr th')[thindex];
                $(header).find('input').prop("checked", false);
            }
        });
        function setCheckBox() {
            if ($('#gvRuleMachine' != null) || $('#gvRuleMachine' != undefined)) {
                let noOfColumn = $('#gvRuleMachine tr td').length;
                let noOfRow = $('#gvRuleMachine tr').length;
                if (noOfColumn > 1 && noOfRow>1) {
                    for (let i = 1; i < noOfColumn; i++) {
                        var rowCount = 0;
                        for (let j = 1; j < noOfRow; j++) {
                            var row = $('#gvRuleMachine tr')[j];
                            var cell = row.children[i];
                            if ($(cell).find('input').prop("checked")) {
                                rowCount++;
                            }
                        }
                        if (rowCount == noOfRow - 1) {
                            let header = $('#gvRuleMachine tr th')[i];
                            $(header).find('input').prop("checked", true);
                        }
                        else {
                            let header = $('#gvRuleMachine tr th')[i];
                            $(header).find('input').prop("checked", false);
                        }
                    }
                }
            }
        }
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                $('[id$=ddlRules]').multiselect({
                    includeSelectAllOption: true
                });
                var Height = $(window).height() - (150);
                $('#gridContainer').css('height', Height);
                setCheckBox();
            });
            $('#gvRuleMachine tr th input[type="checkbox"]').click(function () {
                debugger;
                var tblRow = $('#gvRuleMachine tr');
                var thindex = $(this).closest('th').index();
                if ($(this).prop("checked")) {
                    for (var i = 1; i < tblRow.length; i++) {
                        var td = tblRow[i].children[thindex];
                        $(td).find('input').prop("checked", true);
                    }
                }
                else {
                    for (var i = 1; i < tblRow.length; i++) {
                        var td = tblRow[i].children[thindex];
                        $(td).find('input').prop("checked", false);
                    }
                }
            });
            $('#gvRuleMachine tr td input[type="checkbox"]').click(function () {
                debugger;
                let tblRow = $('#gvRuleMachine tr');
                let thindex = $(this).closest('td').index();
                let count = 0;
                for (var i = 1; i < tblRow.length; i++) {
                    var td = tblRow[i].children[thindex];
                    if ($(td).find('input').prop("checked")) {
                        console.log("count =" + count);
                        count++;
                    }
                }
                console.log("out count =" + count);
                if (count == tblRow.length - 1) {
                    let header = $('#gvRuleMachine tr th')[thindex];
                    $(header).find('input').prop("checked", true);
                }
                else {
                    let header = $('#gvRuleMachine tr th')[thindex];
                    $(header).find('input').prop("checked", false);
                }
            });
            $(window).resize(function () {
                var Height = $(window).height() - (150);
                $('#gridContainer').css('height', Height);
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
