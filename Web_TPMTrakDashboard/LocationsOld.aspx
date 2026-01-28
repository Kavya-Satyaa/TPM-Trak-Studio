<%@ Page Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="LocationsOld.aspx.cs" Inherits="Web_TPMTrakDashboard.LocationsOld" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #map {
            height: 94vh;
            width: 98vw;
        }
    </style>

    <div id="map"></div>

    <script type="text/javascript">
        var locationsList = {};
        var markers = [];
        function initMap() {
            var locationsList = JSON.parse($.ajax({ 'url': "LocationCoordinates.json", 'async': false }).responseText);
            var options = {
                zoom: 2,
                center: { lat: 44.0165, lng: 21.0059 },
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                panControl: true,
                zoomControl: true,
                mapTypeControl: true,
                scaleControl: true,
                streetViewControl: true,
                overviewMapControl: true,
                rotateControl: true
            }
            var map = new google.maps.Map(document.getElementById("map"), options);
            for (var i = 1; i <= Object.keys(locationsList).length; i++) {
                var location = "Location" + i;
                var latLng = new google.maps.LatLng(locationsList[location].coords.lat, locationsList[location].coords.lng);
                var marker = new google.maps.Marker({
                    position: latLng,
                    map: map,
                    animation: google.maps.Animation.DROP,
                    label: locationsList[location].name
                });
                SetInfoWindow(locationsList[location], marker);
                markers.push(marker);
            }
            function SetInfoWindow(props, mrkr) {
                if (props.content) {
                    var infoWindow = new google.maps.InfoWindow({
                        content: props.content
                    });
                    mrkr.addListener('mouseover', function () {
                        infoWindow.open(map, mrkr);
                    });
                    mrkr.addListener('mouseout', function () {
                        infoWindow.close();
                    });
                }
                SetMarkerClick(props, mrkr);
            }
            function SetMarkerClick(propts, mrker) {
                if (propts.dbname) {
                    mrker.addListener('click', function () {
                        window.open("Dashboard.aspx?dbname=" + propts.dbname, "MyTargetWindowName", "", "_self");
                    });
                }
            }
            var markerCluster = new MarkerClusterer(map, markers,
                { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
        }
    </script>

    <script src="https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/markerclusterer.js"></script>
    <script async defer
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBZ1J1dMQjxHwNkcZbIOgObuiUcIhAtwZQ&callback=initMap">
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
