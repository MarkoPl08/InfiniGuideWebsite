
const API = "https://infiniguide.azurewebsites.net/api/";

window.onload = function () {

    var fullName = document.querySelector("#fullname");
    fullName.innerHTML += getCookie("Account").split('@')[0];
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