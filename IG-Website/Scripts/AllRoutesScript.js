function customMarker(lat, lng, name) {
    this.Lat = lat;
    this.Lng = lng;
    this.name = name;
}

function Marker(lat, lng, name, orderNumber) {
    this.Lat = lat;
    this.Lng = lng;
    this.name = name;
    this.orderNumber = orderNumber;
}

function RouteMonument(IDRouteMonument, RouteID, MonumentID, OrderNumber) {
    this.IDRouteMonument = IDRouteMonument;
    this.RouteID = RouteID;
    this.MonumentID = MonumentID;
    this.OrderNumber = OrderNumber;
}

function RouteRestaurant(IDRouteRestaurant, RouteID, RestaurantID, OrderNumber) {
    this.IDRouteRestaurant = IDRouteRestaurant;
    this.RouteID = RouteID;
    this.RestaurantID = RestaurantID;
    this.OrderNumber = OrderNumber;
}

function RouteBar(IDRouteBar, RouteID, BarID, OrderNumber) {
    this.IDRouteBar = IDRouteBar;
    this.RouteID = RouteID;
    this.BarID = BarID;
    this.OrderNumber = OrderNumber;
}

var email = "";
var map;
var routes = [];
var markers = [];

const API = "https://infiniguide.azurewebsites.net/api/";
//const APIi = "https://localhost:44361/api/";

function initMap() {
    const uluru = { lat: 42.640278, lng: 18.108334 };
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 17,
        center: uluru,
    });
}

window.onload = function () {

    fillList()
    getAccount()
    console.log(email)
}

function getAccount() {
    email = getCookie("Account")
}

function fillList() {
    $(document).ready(function () {
        $.ajax({
            url: `${API }routes/get/`,
            datatype: "JSON",
            type: "Get",
            crossDomain: true,
            success: function (data) {
                routes = data;
                var routesList = document.querySelector("#ulRoutes");
                for (var i = 0; i < routes.length; i++) {
                    routesList.innerHTML += `<li class="list-group-item routeItem" id="">
                        <div class="row">
                            <div class="col-lg-10 col-sm-8 placeName" style="font-size:2rem;">${data[i].Name}</div>
                            <div class="col-lg-2"></div>
                            <div class="col-lg-10 col-sm-12 placeName">${data[i].About}</div>
                            <div class="col-lg-1 col-sm-2 placeRemove" style="padding-right:3rem;">
                                <button class="btn btn-primary btn-sm btnRemovePlace" type="button" id="${data[i].IDRoute}" onclick="showRoute(this.id);">Select</button>
                            </div>
                        </div>
                    </li>`;
                }
            }
        });
});


}


function showRoute(id) {
    $(document).ready(function () {
        $.ajax({
            url: `${API}routes/get/${id}`,
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                drawRoute(data);
            }
        });
    });
}

function drawRoute(route) {
    updateMarkers(route);
    initMap();
    //map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
    var infoWindow = new google.maps.InfoWindow();
    var lat_lng = new Array();
    var latlngbounds = new google.maps.LatLngBounds();

    for (i = 0; i < markers.length; i++) {
        var data = markers[i];
        var myLatlng = new google.maps.LatLng(data.Lat, data.Lng);
        lat_lng.push(myLatlng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            label: `${i + 1}`,
            title: data.name
        });

        latlngbounds.extend(marker.position);
        (function (marker, data) {
            google.maps.event.addListener(marker, "click", function (e) {
                infoWindow.setContent(data.timestamp);
                infoWindow.open(map, marker);
            });
        })(marker, data);
    }
    map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);

    //***********ROUTING****************//
    //Initialize the Direction Service
    var service = new google.maps.DirectionsService();

    //Loop and Draw Path Route between the Points on MAP
    for (var i = 0; i < lat_lng.length; i++) {
        if ((i + 1) < lat_lng.length) {
            var src = lat_lng[i];
            var des = lat_lng[i + 1];

            service.route({
                origin: src,
                destination: des,
                travelMode: google.maps.DirectionsTravelMode.WALKING
            }, function (result, status) {
                if (status == google.maps.DirectionsStatus.OK) {

                    //Initialize the Path Array
                    var path = new google.maps.MVCArray();

                    //Set the Path Stroke Color
                    var poly = new google.maps.Polyline({
                        map: map,
                        strokeColor: '#4986E7'
                    });
                    poly.setPath(path);
                    for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                        path.push(result.routes[0].overview_path[i]);
                    }
                }
            });
        }
    }

}

function updateMarkers(route) {
    markers = [];
    var routeMonuments = route.RouteMonuments;
    var routeRestaurants = route.RouteRestaurants;
    var routeBars = route.RouteBars;
    var allMarkers = [];

    console.log(routeMonuments);
    console.log(routeRestaurants);
    console.log(routeBars);

    for (var i = 0; i < routeMonuments.length; i++) {
        var monument = routeMonuments[i].Monument;
        allMarkers.push(new Marker(monument.Lat, monument.Lng, monument.Name, routeMonuments[i].OrderNumber));
    }

    for (var i = 0; i < routeRestaurants.length; i++) {
        var restaurant = routeRestaurants[i].Restaurant;
        allMarkers.push(new Marker(restaurant.Lat, restaurant.Lng, restaurant.Name, routeRestaurants[i].OrderNumber));
    }

    for (var i = 0; i < routeBars.length; i++) {
        var bar = routeBars[i].Bar;
        allMarkers.push(new Marker(bar.Lat, bar.Lng, bar.Name, routeBars[i].OrderNumber));
    }

    console.log(allMarkers);

    var size = allMarkers.length;

    for (var i = 0; i < size; i++) {
        for (var j = 0; j < size; j++) {
            if (i == allMarkers[j].orderNumber) {
                var marker = allMarkers[j];
                markers.push(new customMarker(marker.Lat, marker.Lng, marker.Name));
            }
        }
    }
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
            //return c.substring(nameEQ.length, c.length);
            var cook = c.split('=');
        return cook[2];
    }
    return null;
}