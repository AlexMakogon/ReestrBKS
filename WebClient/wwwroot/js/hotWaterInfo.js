﻿var subjectId = '';
var personId = '';
var accountNumber = '';

$('document').ready(function () {
    var params = window.location.search.replace('?','').split('&');

    for (var i = 0; i < params.length; i++) {
        if (params[i].split("=")[0] == 'subjectId')
            subjectId = params[i].split("=")[1];
        if (params[i].split("=")[0] == 'personId')
            personId = params[i].split("=")[1];
        if (params[i].split("=")[0] == 'accountNumber')
            accountNumber = params[i].split("=")[1];
    }

    $('.input-group.date').datepicker({
        format: "mm-yyyy",
        minViewMode: 1,
        maxViewMode: 2,
        language: "ru"
    });

    $('.input-group.date').on('changeDate', function (e) {
        $(this).datepicker('hide');
    });

    $('.search-control').on('keypress', function (e) {
        if (e.which == 13) {
            LoadAbstractValues();
        }
    });

    LoadAbstractValues();
});

function LoadAbstractValues() {
    var startDate = $("#periodStart").val();
    var finishDate = $("#periodFinish").val();

    ShowLoader();
    $.ajax({
        url: "/HotWater/GetInfo",
        method: "post",
        async: true,
        data: { subjectId: subjectId, personId: personId, accountNumber: accountNumber, startDate: startDate, finishDate: finishDate },
        cache: false,
        success: function (data) {
            console.log(data);
            $("#accountNumber").html(accountNumber);
            $("#address").html(GetAddress(data.subject));
            $("#person").html(data.person.name);

            $("#abstractLines tbody").html('');
            data.lines.forEach(function (item, idx, arr) {
                $("#abstractLines tbody").append(getLineInfoRow(item));
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Ошибка, не удалось загрузить данные" + jqXHR.responseText);
        },
        complete: function (data) {
            HideLoader();
        }
    });
}

function getLineInfoRow(item) {
    var rowPattern = '<tr>' +
        '<td>{year}</td>' +
        '<td>{month}</td>' +
        '<td>{amountType}</td>' +
        '<td>{incBalance}</td>' +
        '<td>{incBalanceDebit}</td>' +
        '<td>{incBalanceCredit}</td>' +
        '<td>{heating}</td>' +
        '<td>{hotWater}</td>' +
        '<td>{hotWaterTE}</td>' +
        '<td>{hotWaterCommonTE}</td>' +
        '<td>{hotWaterCommonTN}</td>' +
        '<td>{hotWaterIncreaseTN}</td>' +
        '<td>{coldWater}</td>' +
        '<td>{total}</td>' +
        '<td>{penalty}</td>' +
        '<td>{outBalance}</td>' +
        '<td>{outBalanceDebit}</td>' +
        '<td>{outBalanceCredit}</td>' +
        '</tr>';

    return rowPattern.replace(/{year}/g, item.year)
        .replace(/{month}/g, item.month)
        .replace(/{amountType}/g, item.amountType.name)
        .replace(/{incBalance}/g, item.incBalance)
        .replace(/{incBalanceDebit}/g, item.incBalanceDebit)
        .replace(/{incBalanceCredit}/g, item.incBalanceCredit)
        .replace(/{heating}/g, item.heating)
        .replace(/{hotWater}/g, item.hotWater)
        .replace(/{hotWaterTE}/g, item.hotWaterTE)
        .replace(/{hotWaterCommonTE}/g, item.hotWaterCommonTE)
        .replace(/{hotWaterCommonTN}/g, item.hotWaterCommonTN)
        .replace(/{hotWaterIncreaseTN}/g, item.hotWaterIncreaseTN)
        .replace(/{coldWater}/g, item.coldWater)
        .replace(/{total}/g, item.total)
        .replace(/{penalty}/g, item.penalty)
        .replace(/{outBalance}/g, item.outBalance)
        .replace(/{outBalanceDebit}/g, item.outBalanceDebit)
        .replace(/{outBalanceCredit}/g, item.outBalanceCredit);
}

function ClearButtonClick() {
    $("#periodStart").val("");
    $("#periodFinish").val("");
    LoadAbstractValues();
}

function GetReport(reportName) {
    var startDate = $("#periodStart").val();
    var finishDate = $("#periodFinish").val();

    var hrefPattern = "/HotWater/GetReport?subjectId={subjectId}&personId={personId}&accountnumber={accountNumber}&reportName={reportName}&startDate={startDate}&finishDate={finishDate}";
    var url = hrefPattern
        .replace("{subjectId}", subjectId)
        .replace("{personId}", personId)
        .replace("{accountNumber}", accountNumber)
        .replace("{startDate}", startDate)
        .replace("{finishDate}", finishDate)
        .replace("{reportName}", reportName);

    $.ajax({
        url: url,
        method: "get",
        success: function (data) {
            location.href = url;
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}

function goToReestr() {
    location.href = "/HotWater";
}