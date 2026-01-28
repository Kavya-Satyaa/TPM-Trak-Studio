<%@ Page Title="Locations" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="Web_TPMTrakDashboard.Locations" %>

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
            var locationsList;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: '<%= ResolveUrl("Locations.aspx/GetAllLocationDetails")%>',
                success: function (result) {
                    locationsList = result.d;
                },
                error: function (errorMessage) {
                    alert("Error : " + errorMessage);
                }
            });

            var options = {
                zoom: 5,
                center: { lat: 22.0574, lng: 78.9382 },
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

            for (var i = 0; i < locationsList.length; i++) {
                var latLng = new google.maps.LatLng(locationsList[i].Latitude, locationsList[i].Longitude);
                var marker = new google.maps.Marker({
                    position: latLng,
                    map: map,
                    animation: google.maps.Animation.DROP,
                    label: locationsList[i].Name
                });
                SetInfoWindow(locationsList[i], marker);
                markers.push(marker);
            }

            function PlotLocationsOnMap(locationsList) {
                if (locationsList) {
                    debugger;
                    for (var i = 0; i < locationsList.length; i++) {
                        var latLng = new google.maps.LatLng(locationsList[i].Latitude, locationsList[i].Longitude);
                        var marker = new google.maps.Marker({
                            position: latLng,
                            map: map,
                            animation: google.maps.Animation.DROP,
                            label: locationsList[i].Name
                        });
                        SetInfoWindow(locationsList[i], marker);
                        markers.push(marker);
                    }
                }
            }

            function SetInfoWindow(props, mrkr) {
                let infoContent;
                if (props.Details) {
                    if (props.OEEString) {
                        infoContent = `${props.Details}<br><strong>OEE - </strong>${props.OEEString}`;
                    } else {
                        infoContent = props.Details;
                    }
                    var infoWindow = new google.maps.InfoWindow({
                        content: infoContent
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
                if (propts.dbName) {
                    mrker.addListener('click', function () {
                        window.open("SignIn.aspx?dbname=" + propts.dbName, "MyTargetWindowName", "", "_self");
                    });
                }
            }

            var markerCluster = new MarkerClusterer(map, markers,
                { imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m' });
        }
    </script>

    <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js"></script>
    <script async defer
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBZ1J1dMQjxHwNkcZbIOgObuiUcIhAtwZQ&callback=initMap">
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
