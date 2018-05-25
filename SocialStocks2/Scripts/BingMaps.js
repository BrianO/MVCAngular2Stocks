


function updateHeight() {
    var div = $('#mapDiv');

    div.css('height', div.width() - 100);
}

function AddFail() {
    alert("Failure retrieving data. Login may have expired.");
}

function AddComplete(context) {
    $("#CustomerName").val("");
    $("#CustomerAddress").val("");
    $("#CustomerPhone").val("");

    $("#CustDiv").html(context.Data);
}

function selectCustomer(item) {
    $("#txtQuery").val(item);

    ClickGeocode();
}

function getZipMap() {
    var zip = $("#yourZipCode").val();

    if (zip.length > 4) {
        getBing(zip);
    }
}

var bingKey = "AiC1kEtTevT4tM4tOQmfZfuhkk2QsjEKIODNaqPcqLO0_CYvwGpx_ArCSxQYlpv_";

function getBing(zip) {

    var q = "https://dev.virtualearth.net/REST/v1/Locations?postalCode=" +
            zip +
            "&includeNeighborhood=0&maxResults=5&key=" + bingKey;

    var centerPoint = "";

    $.ajax({
        url: q,
        dataType: "jsonp",
        jsonp: "jsonp",
        success: function (data) {

            if (data.resourceSets[0].resources[0].point) {
                centerPoint =
                data.resourceSets[0].resources[0].point.coordinates[0] +
                 "," +
                data.resourceSets[0].resources[0].point.coordinates[1];

                $("#bCoordinates").html("Your coordinates :" + centerPoint);

                var mapsLink =
                    "https://dev.virtualearth.net/REST/v1/Imagery/Map/AerialWithLabels/" +
                    centerPoint +
                    "/15?mapSize=500,500&key=" +
                    bingKey;

                $("#bMap").attr("src", mapsLink);
            }
            else {
                $("#bCoordinates").html("Account is rate-limited.");

                var mapsLink =
               "https://dev.virtualearth.net/REST/v1/Imagery/Map/AerialWithLabels/"
               + zip
               + "?mapSize=500,500&key="
               + bingKey;

                $("#bMap").attr("src", mapsLink);
            }
        }
    });
};


var map = null;

function GetMap() {
    // Initialize the map
    map = new Microsoft.Maps.Map(document.getElementById("mapDiv"),
        {
            credentials: bingKey,
            mapTypeId: Microsoft.Maps.MapTypeId.road
        });
}

function ClickGeocode(credentials) {
    map.getCredentials(MakeGeocodeRequest);
}

function MakeGeocodeRequest(credentials) {

    var geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations?query=" + encodeURI(document.getElementById('txtQuery').value) + "&output=json&jsonp=GeocodeCallback&key=" + credentials;

    CallRestService(geocodeRequest);
}

function GeocodeCallback(result) {
    //alert("Found location: " + result.resourceSets[0].resources[0].name);

    if (result &&
           result.resourceSets &&
           result.resourceSets.length > 0 &&
           result.resourceSets[0].resources &&
           result.resourceSets[0].resources.length > 0) {
        // Set the map view using the returned bounding box
        var bbox = result.resourceSets[0].resources[0].bbox;
        var viewBoundaries = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(bbox[0], bbox[1]), new Microsoft.Maps.Location(bbox[2], bbox[3]));
        map.setView({ bounds: viewBoundaries });

        // Add a pushpin at the found location
        var location = new Microsoft.Maps.Location(result.resourceSets[0].resources[0].point.coordinates[0], result.resourceSets[0].resources[0].point.coordinates[1]);
        var pushpin = new Microsoft.Maps.Pushpin(location);
        map.entities.push(pushpin);
    }
}

function CallRestService(request) {
    var script = document.createElement("script");
    script.setAttribute("type", "text/javascript");
    script.setAttribute("src", request);
    document.body.appendChild(script);
}

$(function () {
  //  GetMap()

    updateHeight();
  }
);

$(window).resize(updateHeight);
