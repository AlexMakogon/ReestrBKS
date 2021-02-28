$('document').ready(function () {
    $('.search-control').on('keypress', function (e) {
        if (e.which == 13) {
            LoadAbstractLines(1);
        }
    });

    LoadAbstractLines(1);
});

function LoadAbstractLines(pageNumber) {
    var street = $("#street").val();
    var house = $("#house").val();
    var apartment = $("#apartment").val();
    var room = $("#room").val();
    var accountNumber = $("#accountNumber").val();
    var person = $("#person").val();
    var itemsOnPage = $("#itemsOnPage").val();
    

    if (itemsOnPage == undefined)
        itemsOnPage = 20;

    var filter = {
        Street: street,
        House: house,
        Apartment: apartment,
        Room: room,
        AccountNumber: accountNumber,
        Person: person
    };

    if (street == "" && house == "" && apartment == "" && room == "" && accountNumber == "" && person == "") {
        $('#emptyFilter').show();
        $('#abstractLines tbody').html('');
        $("#pagination").html('');
        return;
    }
    else {
        $('#emptyFilter').hide();
    }

    ShowLoader();
    $.ajax({
        url: "/ColdWater/GetAbstractLines",
        method: "post",
        async: true,
        data: { filter: filter, pageNumber: pageNumber, itemsOnPage: itemsOnPage },
        cache: false,
        success: function (data) {
            console.log(data);
            var holder = $('#abstractLines tbody');
            $(holder).html('');

            data.lines.forEach(function (item, idx, arr) {
                RenderAbstractLine(holder, item);
            });

            RenderPagination(data.pages, data.pageNumber, data.itemsOnPage);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Ошибка, не удалось загрузить реестр." + jqXHR.responseText);
        },
        complete: function (data) {
            HideLoader();
        }
    });
}

function RenderPagination(pages, pageNumber, itemsOnPage) {
    var holder = $("#pagination");
    var pagePattern = '<li class="page-item {active}"><a class="page-link" onclick="PageClick({number})">{number}</a></li>';

    holder.html('');
    pages.forEach(function (item, idx, arr) {
        var active;
        if (item == pageNumber) active = "active"
        else active = "";

        $(holder).append(pagePattern
            .replace("{active}", active)
            .replace(/{number}/g, item));
    });

    holder.append('<li class="page-item"><select id="itemsOnPage" class="form-control" onchange="ItemsOnPageChange()"></select></li>');
    var pagecounts = [5, 10, 15, 20, 50, 100];
    var pcPattern = '<option value="{value}" {selected}>{value}</option>';
    var selected;
    pagecounts.forEach(function (item, idx, arr) {
        if (itemsOnPage == item) selected = "selected"
        else selected = "";
        $("#itemsOnPage").append(pcPattern
            .replace(/{value}/g, item)
            .replace("{selected}", selected));
    });
}

function ItemsOnPageChange() {
    LoadAbstractLines(1);
}

function PageClick(pageNumber) {
    LoadAbstractLines(pageNumber);
}

function RenderAbstractLine(holder, item) {
    var linePattern =
        '<tr onclick="OpenInfo({SId},{PId},{Account})">' +
            '<td>{Account}</td>' + 
            '<td>{Person}</td>' +
            '<td>{Address}</td>' +
        '<tr>';

    $(holder).append(linePattern.replace('{YearMonth}', item.year + '/' + item.month)
        .replace(/{SId}/g, item.subject.id)
        .replace(/{PId}/g, item.person.id)
        .replace(/{Account}/g, item.accountNumber)
        .replace(/{Person}/g, item.person.name)
        .replace(/{Address}/g, GetAddress(item.subject)));
}

function OpenInfo(sid, pid, accountNumber) {
    window.open("../ColdWater/Info?subjectId=" + sid + "&personId=" + pid + "&accountNumber=" + accountNumber);
}

function ClearButtonClick() {
    $("#street").val('');
    $("#house").val('');
    $("#apartment").val('');
    $("#room").val('');
    $("#accountNumber").val('');
    $("#person").val('');
    $("#itemsOnPage").val('');
    LoadAbstractLines(1);
}