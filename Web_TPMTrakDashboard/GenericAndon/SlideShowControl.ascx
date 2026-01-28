<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlideShowControl.ascx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.SlideShowControl" %>

<style>
    .carousel-indicators {
        visibility: hidden;
    }

    #myCarousel {
        height: 100%;
    }

        #myCarousel .item {
            height: 100%;
        }

        #myCarousel .carousel-inner {
            height: 100%;
        }
        #myCarousel img, #myCarousel video {
            height: 100%;
        }

    .hideConrol {
        display: none;
    }
</style>
<div id="slideShowContainer" style="height: 90vh">
   <%-- <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnPost" ClientIDMode="Static" OnClick="btnPost_Click" Style="display: none" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPost" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <div id="myCarousel" class="carousel slide" data-bs-ride="carousel">
        <asp:Literal ID="ltlCarouselIndicators" runat="server" />
        <!-- Images-->
        <div class="carousel-inner" role="listbox">
            <asp:Literal ID="ltlCarouselImages" runat="server" />
        </div>
        <!-- Left Right Arrows -->
        <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left" aria-hidden="true" style="visibility: hidden"></span>
            <span class="sr-only">Previous</span>
        </a>
        <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right" aria-hidden="true" style="visibility: hidden"></span>
            <span class="sr-only">Next</span>
        </a>
    </div>
</div>
<script type="text/javascript">
  
    
</script>
