<%@ Page Title="" Language="C#" MasterPageFile="~/WebAndon/AndonMaster.Master" AutoEventWireup="true" CodeBehind="ImageAndVideoPage.aspx.cs" Inherits="Web_TPMTrakDashboard.WebAndon.ImageAndVideoPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%--  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>

     <div class="container-fluid">
        <div id="myCarousel" class="carousel slide" data-ride="carousel">
            <asp:Literal ID="ltlCarouselIndicators" runat="server" />
            <!-- Images-->
            <div class="carousel-inner" role="listbox">
                <asp:Literal ID="ltlCarouselImages" runat="server" />
            </div>
            <!-- Left Right Arrows -->
            <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                <span class="sr-only">Previous</span>
            </a>
            <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                <span class="sr-only">Next</span>
            </a>
        </div>
    </div>

    <script type="text/javascript">
        var countItem = 0;
        $('.carousel').carousel({
            pause: "false"
        });
        var screenH = $(window).height() - 120;//screen.availHeight - 100;
        var screenW = $(window).width() - 30;//screen.availWidth - 5;
        $(".makeStyle").css("height", screenH);//.height = screenH;
        $(".makeStyle").css("width", "auto");//.width = screenW;
        $(document).ready(function () {

            $("#myCarousel").on('slid.bs.carousel', function () {
                countItem++;
                $(".makeStyle").css("height", screenH);//.height = screenH;
                $(".makeStyle").css("width", "auto");//.width = screenW;
                var video = $("#myCarousel .item.active").children("video");
                var isVideo = (video.length > 0);
                if (isVideo) {
                    $("#myCarousel").carousel("pause");
                    video.get(0).play();
                }
                if (countItem == $("#myCarousel .item").length) {
                    window.location.href = "Andon.aspx";
                }
            });
        })

        $("video").each(function () {
            this.addEventListener('ended', myHandler, false);
            this.addEventListener('error', myHandler, false);
        });

        function myHandler(e) {
            // What you want to do after the event
            $("#myCarousel").carousel("cycle");
        }

    </script>
</asp:Content>
