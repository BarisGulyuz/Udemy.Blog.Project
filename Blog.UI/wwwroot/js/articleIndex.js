$(document).ready(function () {

    /*DATATABLE START HERE */
    const dataTable = $('#articles-table').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center mb-3'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Yeni Makale',
                attr: {
                    id: 'btn-add'
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {
                    let url = window.location.href
                    url.replace("/Index", "")
                    window.open(`${url}/Add`, "_self")
                    
                }
            },
            {
                text: 'Safayı Yenile',
                attr: {
                    id: 'btn-refresh'
                },
                className: 'btn btn-default',
                action: function (e, dt, node, config) {
                    window.location.reload()
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json"
        }
    });
    /*DATATABLE END HERE */
    /*POST START HERE */

    /*POST END HERE */
    /*DELETE START HERE */
    $(document).on('click', '#btn-article-delete', function (event) {
        const id = $(this).attr('data-id')
        const tableRow = $(`[name="${id}"]`)
        const articleName = tableRow.find('td:eq(3)').text()
        event.preventDefault()
        Swal.fire({
            title: 'Silmek istediğine emin misin?',
            text: `"${articleName}" adlı makale silinecektir!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet',
            cancelButtonText: 'Hayır'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { articleId : id },
                    url: '/Admin/Article/Delete',
                    success: function (data) {
                        const result = jQuery.parseJSON(data)
                        if (result.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                `${articleName} adlı makale silindi`,
                                'success'
                            )
                            dataTable.row(tableRow).remove().draw()
                        }

                    },
                    error: function (errors) {
                        console.log(errors)
                    }
                })

            }
        })
    })
    /*DELETE END HERE */

    /*UPDATE START HERE*/

 
})


