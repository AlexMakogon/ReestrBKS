var subjectId = '';
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
        url: "/CommonHouse/GetInfo",
        method: "post",
        async: true,
        data: { subjectId: subjectId, personId: personId, accountNumber: accountNumber, startDate: startDate, finishDate: finishDate },
        cache: false,
        success: function (data) {
            console.log(data);
            $("#address").html(GetAddress(data.subject));

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
        '<td>{accountNumber}</td>' +
        '<td>{incCharge}</td>' +
        '<td>{incBalance}</td>' +
        '<td>{incPenalty}</td>' +
        '<td>{chargeCorrectBalance}</td>' +
        '<td>{chargeCorrectPenalty}</td>' +
        '<td>{chargeMonth}</td>' +
        '<td>{chargeBalance}</td>' +
        '<td>{chargePenalty}</td>' +
        '<td>{paymentTotal}</td>' +
        '<td>{paymentBalance}</td>' +
        '<td>{paymentPenalty}</td>' +
        '<td>{outBalance}</td>' +
        '<td>{outPenalty}</td>' +
        '</tr>';

    return rowPattern.replace(/{year}/g, item.year)
        .replace(/{month}/g, item.month)
        .replace(/{accountNumber}/g, item.accountNumber)
        .replace(/{incCharge}/g, item.incCharge)
        .replace(/{incBalance}/g, item.incBalance)
        .replace(/{incPenalty}/g, item.incPenalty)
        .replace(/{chargeCorrectBalance}/g, item.chargeCorrectBalance)
        .replace(/{chargeCorrectPenalty}/g, item.chargeCorrectPenalty)
        .replace(/{chargeMonth}/g, item.chargeMonth)
        .replace(/{chargeBalance}/g, item.chargeBalance)
        .replace(/{chargePenalty}/g, item.chargePenalty)
        .replace(/{paymentTotal}/g, item.paymentTotal)
        .replace(/{paymentBalance}/g, item.paymentBalance)
        .replace(/{paymentPenalty}/g, item.paymentPenalty)
        .replace(/{outBalance}/g, item.outBalance)
        .replace(/{outPenalty}/g, item.outPenalty);
}

function ClearButtonClick() {
    $("#periodStart").val("");
    $("#periodFinish").val("");
    LoadAbstractValues();
}

function GetReport(reportName) {
    var startDate = $("#periodStart").val();
    var finishDate = $("#periodFinish").val();

    var hrefPattern = "/CommonHouse/GetReport?subjectId={subjectId}&personId={personId}&accountnumber={accountNumber}&reportName={reportName}&startDate={startDate}&finishDate={finishDate}";
    var url = hrefPattern
        .replace(/{subjectId}/g, subjectId)
        .replace(/{personId}/g, personId)
        .replace(/{accountNumber}/g, accountNumber)
        .replace(/{startDate}/g, startDate)
        .replace(/{finishDate}/g, finishDate)
        .replace(/{reportName}/g, reportName);

    console.log(url);

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
    location.href = "/CommonHouse";
}