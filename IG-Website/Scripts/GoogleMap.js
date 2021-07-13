function place(placeID, placeType, idPlace) {
    this.placeID = placeID;
    this.placeType = placeType;
    this.idPlace = idPlace;
}

function customMarker(lat, lng, name, placeId, placeType, idPlace) {
    this.Lat = lat;
    this.Lng = lng;
    this.name = name;
    this.placeId = placeId;
    this.placeType = placeType;
    this.idPlace = idPlace;
}

function RoutePlace(PlaceID, OrderNumber) {
    this.PlaceID = PlaceID;
    this.OrderNumber = OrderNumber;
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

function AccountRoute(IDAccountRoute, RouteID, AccountID, RouteType) {
    this.IDAccountRoute = IDAccountRoute;
    this.RouteID = RouteID;
    this.AccountID = AccountID;
    this.RouteType = RouteType;
}

function Route(IDRoute, Name, About) {
    this.IDRoute = IDRoute;
    this.Name = Name;
    this.About = About;
    this.PicturePath = defaultPicturePath;
}

function Account(IDAccount, Email) {
    this.IDAccount = IDAccount;
    this.Email = Email;
}

const API = "https://infiniguide.azurewebsites.net/api/";
const monumentIcon = `<i class="fas fa-landmark"></i>`;
const restaurantIcon = `<i class="fas fa-utensils"></i>`;
const barIcon = `<i class="fas fa-mug-hot"></i>`;
const defaultPicturePath = `https://images.unsplash.com/photo-1589472501747-1660aeed4d80?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1051&q=80`;
const placesLimit = 8;

var currentPlaces = 0;
var account;
var map;

var markers = [];
var monuments = [];
var restaurants = [];
var bars = [];

window.onload = function () {
    loadPlaces();
    initMap();
    //console.log(getCookie("Account"));
}

function initMap() {
    const uluru = { lat: 42.640278, lng: 18.108334 };
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 17,
        center: uluru,
    });
}

function drawRoute() {
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
            //icon:"../Images/food.png",
            //icon:"icon.png",
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

function loadPlaces() {

    $(document).ready(function () {
        $.ajax({
            url: `${API}monuments/get/`,
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                monuments = data;
                var btnMonuments = document.querySelector("#selectMonuments");
                for (var i = 0; i < data.length; i++) {
                    btnMonuments.innerHTML += `<option value="${i}">${data[i].Name}</option>`;
                }
            }
        });
    });

    $(document).ready(function () {
        $.ajax({
            url: `${API}restaurants/get/`,
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                restaurants = data;
                var btnRestaurants = document.querySelector("#selectRestaurants");
                for (var i = 0; i < data.length; i++) {
                    btnRestaurants.innerHTML += `<option value="${i}">${data[i].Name}</option>`;
                }
            }
        });
    });

    $(document).ready(function () {
        $.ajax({
            url: `${API}bars/get/`,
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                bars = data;
                var btnBars = document.querySelector("#selectBars");
                for (var i = 0; i < data.length; i++) {
                    btnBars.innerHTML += `<option value="${i}">${data[i].Name}</option>`;
                }
            }
        });
    });
}

function updateRoute() {
    initMap();
    if (currentPlaces <= 0) {
        return;
    }
    drawRoute();
}

function monumentSelected() {
    var e = document.getElementById("selectMonuments");
    var value = e.options[e.selectedIndex].value;
    addSelectedMonument(value);
}

function addSelectedMonument(value) {
    var name = monuments[value].Name;
    var id = `monument${monuments[value].IDMonument}`;
    addSelectedPlace(id, name, monumentIcon, monuments[value].IDMonument, "monument", monuments[value]);
}

function restaurantSelected() {
    var e = document.getElementById("selectRestaurants");
    var value = e.options[e.selectedIndex].value;
    addSelectedRestaurant(value);
}

function addSelectedRestaurant(value) {
    var name = restaurants[value].Name;
    var id = `restaurant${restaurants[value].IDRestaurant}`;
    addSelectedPlace(id, name, restaurantIcon, restaurants[value].IDRestaurant, "restaurant", restaurants[value]);
}

function barSelected() {
    var e = document.getElementById("selectBars");
    var value = e.options[e.selectedIndex].value;
    addSelectedBar(value);
}

function addSelectedBar(value) {
    console.log(currentPlaces + " " + placesLimit);
    var name = bars[value].Name;
    var id = `bar${bars[value].IDBar}`;
    addSelectedPlace(id, name, barIcon, bars[value].IDBar, "bar", bars[value]);
}

function addSelectedPlace(placeId, placeName, icon, id, placeType, newPlace) {
    if (currentPlaces >= placesLimit) {
        return;
    }

    for (var i = 0; i < markers.length; i++) {
        if (markers[i].placeId === placeId) {
            return;
        }
    }
    currentPlaces++;

    var placeItemsList = document.querySelector("#ulRoute");
    placeItemsList.innerHTML += `<li class="list-group-item placeItem" id="${placeId}">
        <div class="row">
            <div class="col-lg-1 col-sm-1 placeIcon">${icon}</div>
            <div class="col-lg-7 col-sm-7 placeName">${placeName}</div>
            <div class="col-lg-3 col-sm-3 placeRemove">
                <button class="btn btn-primary btn-sm btnRemovePlace" type="button" id="${placeId}" onclick="removePlace(this.id);">Remove</button>
            </div>
        </div>
    </li>`;

    var marker = new customMarker(newPlace.Lat, newPlace.Lng, placeName, placeId, placeType, id);
    markers.push(marker);

    console.log(marker);
    console.log(markers);

    updateRoute();
}

function removePlace(id) {
    var div = document.getElementById(id);
    div.remove();

    currentPlaces--;

    for (var i = 0; i < markers.length; i++) {
        if (markers[i].placeId === id) {
            var index = markers[i];
            console.log(id);
            console.log(index);
            markers.splice(i, 1);
        }
    }
    console.log(markers);

    updateRoute();
}

function saveRoute() {
    var routeName = document.getElementById('routeName');
    var routeAbout = document.getElementById('routeAbout');

    if (routeAbout.value == null || routeName == null || routeAbout.value == "" || routeName == "") {
        alert('Please enter valid Route name and About.');
        return;
    }
    if (currentPlaces < 3) {
        alert('Please select more then 2 places in the route.');
        return;
    }

    saveAdminRoute();
    return;

    //if (getCookie('Email') == "admin@infiniguide.hr") {
    //    saveAdminRoute();
    //    return;
    //}
    //savePersonalRoute();
}

function savePersonalRoute() {
    var routeMonuments = [];
    var routeRestaurants = [];
    var routeBars = [];

    var accountRoute = new AccountRoute(1, 1, account.IDAccount, 'MyRoute');

    for (var i = 0; i < markers.length; i++) {
        var place = markers[i];
        switch (place.placeType) {
            case 'monument':
                routeMonuments.push(new RouteMonument(1, 1, place.idPlace, i))
                break;
            case 'restaurant':
                routeRestaurants.push(new RouteRestaurant(1, 1, place.idPlace, i))
                break;
            case 'bar':
                routeBars.push(new RouteBar(1, 1, place.idPlace, i))
                break;
            default:
        }
    }
    var routeName = document.getElementById('routeName');
    var routeAbout = document.getElementById('routeAbout');

    var route = new Route(1, routeName.value, routeAbout.value);

    $.ajax({
        type: "POST",
        url: `${API}routes/post/`,
        data: { route, routeMonuments, routeRestaurants, routeBars, accountRoute },
        dataType: "json",
        success: function () {
            alert('Route Saved.');
        }
    });
}

function saveAdminRoute() {
    var routeMonuments = [];
    var routeRestaurants = [];
    var routeBars = [];

    for (var i = 0; i < markers.length; i++) {
        var place = markers[i];
        switch (place.placeType) {
            case 'monument':
                routeMonuments.push(new RouteMonument(1, 1, place.idPlace, i))
                break;
            case 'restaurant':
                routeRestaurants.push(new RouteRestaurant(1, 1, place.idPlace, i))
                break;
            case 'bar':
                routeBars.push(new RouteBar(1, 1, place.idPlace, i))
                break;
            default:
        }
    }
    var routeName = document.getElementById('routeName');
    var routeAbout = document.getElementById('routeAbout');

    var route = new Route(1, routeName.value, routeAbout.value);

    $.ajax({
        type: "POST",
        url: `${API}routes/post/`,
        data: { route, routeMonuments, routeRestaurants, routeBars },
        dataType: "json",
        success: function () {
            alert('Route Saved.');
        }
    });
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

//google.maps.event.addDomListener(window, 'load', initialize);