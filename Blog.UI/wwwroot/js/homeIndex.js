$(document).ready(function () {
    $('#articles-table').dataTable({
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json"
        },
        "order": [[4, "desc"]] //dataTable tarihe göre sıralama
    })
})