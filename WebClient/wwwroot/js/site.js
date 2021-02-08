// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// item - объект класса Subject
function GetAddress(item) {
    var address = item.street.name + " ул., д." + item.house;
    if (item.apartment != null)
        address += ", кв." + item.apartment;
    if (item.room != null)
        address += ", ком." + item.room;

    return address;
}

function ShowLoader() {
    $(".loading").show();
}

function HideLoader() {
    $(".loading").hide();
}
