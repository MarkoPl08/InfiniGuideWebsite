var markers = [{
    "timestamp": 'Zvonik',
    "latitude": '42.64098175826185',
    "longitude": '18.110589757561684',
    "description": 'Zvonik ebate'
},
{
    "timestamp": 'Orlandov stup',
    "latitude": '42.64092503255528',
    "longitude": '18.110349029302597',
    "description": 'Stupina najveća'
},
{
    "timestamp": 'Buggy Safari',
    "latitude": '42.6408342713172',
    "longitude": '18.11005935072899',
    "description": 'Buggy Buggy'
},
{
    "timestamp": 'Pizzeria Olaia',
    "latitude": '42.64063153785614',
    "longitude": '18.110019117593765',
    "description": 'Pizza'
},
{
    "timestamp": 'King\'s Landing',
    "latitude": '42.64088458462857',
    "longitude": '18.109329789876938',
    "description": 'GoT!'
}
];

function mapica() {
    var mapOptions = {
        center: new google.maps.LatLng(markers[0].Lat, markers[0].Lng),
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
    var infoWindow = new google.maps.InfoWindow();
    var lat_lng = new Array();
    var latlngbounds = new google.maps.LatLngBounds();

    for (i = 0; i < markers.length; i++) {
        var data = markers[i]
        var myLatlng = new google.maps.LatLng(data.Lat, data.Lng);
        lat_lng.push(myLatlng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            //icon:"../Images/food.png",
            //icon:"icon.png",
            title: data.timestamp + data.description
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

var routes = [];

function updateRoutes() {
    $(document).ready(function () {
        $.ajax({
            url: "https://localhost:44361/api/routes/",
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                debugger;
                routes = data;
            }
        });
    });

    $(document).ready(function () {
        $.ajax({
            url: "https://localhost:44361/api/monuments/",
            datatype: "JSON",
            type: "Get",
            success: function (data) {
                debugger;
                markers = data;
            }
        });
    });

}

function updateRoute() {

    markers = routes[0].places;
}

window.onload = function () {
    updateRoutes();
}

//google.maps.event.addDomListener(window, 'load', initialize);