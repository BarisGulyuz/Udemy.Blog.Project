
/* NOTLAR
 * DataTable ve button ayarlamaları
 * PartialViewler controller içinden PartiViews("name") olark return edildi ardından jquery ile modallara bağlanıldı
 * Ardında Get,Post,Delete işlemleri
 * Partia'ın string dönmesi için controller extension yazıldı
 * 
 * */

$(document).ready(function () {

    /*DATATABLE START HERE */
    $('#category-table').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center mb-3'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Yeni Kategori',
                attr: {
                    id: 'btn-add'
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {
                }
            },
            {
                text: 'Safayı Yenile',
                attr: {
                    id: 'btn-refresh'
                },
                className: 'btn btn-indo',
                action: function (e, dt, node, config) {
                    window.location.reload()
                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-info',
                action:
                    function (e, dt, node, config) {
                        $.ajax({
                            type: 'GET',
                            url: '/Admin/Category/GetAllCategories',
                            contentType: 'application/json',
                            beforeSend: function () {
                                $('#category-table').hide()
                                $('.spinner-border').show()
                            },
                            success: function (categoryData) {
                                const categories = jQuery.parseJSON(categoryData)
                                console.log(categories)
                                if (categories.ResultStatus === 0) {
                                    let tableBody = ""
                                    $.each(categories.Categories.$values, function (key, value) {
                                        tableBody += `<tr name="${value.Id}">
                                                <td>${value.Id}</td>
                                                <td>${value.Title}</td>
                                                <td>${value.Description}</td>
                                                <td>${ısActiveSpan(value.IsActive)}</td>
                                                <td>${ısDeleteSpan(value.IsDeleted)}</td>
                                                <td>${value.Note}</td>
                                                <td>${value.CreatedByName}</td>
                                                <td>${converToShortDate(value.CreatedDate)}</td>
                                                <td>${value.ModifiedByName}</td>
                                                <td>${converToShortDate(value.ModifiedDate)}</td>
                                                  <td>
                                                        <div class="btn-group">
                                                            <a id="btn-category-delete" class="btn btn-outline-danger btn-sm" data-id="${value.Id}"> <span class="fas fa-minus-circle"></span> </a>
                                                            <a id="btn-category-update" class="btn btn-outline-warning btn-sm"><span class="fas fa-edit"></span> </a>
                                                        </div>
                                                 </td>
                                                                                                </tr>`
                                    })
                                    $('#category-table tbody').replaceWith(tableBody)
                                    $('.spinner-border').hide(500)
                                    $('#category-table').fadeIn(1500)
                                }
                            },
                            error: function () {
                                $('.spinner-border').hide(500)
                                $('#category-table').fadeIn(1500)
                                toastr.error('İŞLEM BAŞARISIZ', "HATA")
                            }

                        })
                    }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json"
        }
    });
    /*DATATABLE END HERE */
    /*POST START HERE */
    $(function () {
        const modalDiv = $('#category-modal')
        const url = '/Admin/Category/Add'

        $('#btn-add').click(function () {
            $.get(url).done(function (data) {
                modalDiv.html(data)
                modalDiv.find(".modal").modal('show')
            })
        })

        modalDiv.on('click', '#btnSave', function (event) {
            event.preventDefault()
            const form = $('#category-add-form')
            const actionUrl = form.attr('action')
            const dataToSend = form.serialize()
            $.post(actionUrl, dataToSend).done(function (categoryData) {
                const category = jQuery.parseJSON(categoryData)
                const newFormBody = $('.modal body', category.CategoryPartial)
                modalDiv.find('.modal body').replaceWith(newFormBody)
                const isValid = newFormBody.find('[name="IsValid"]').val() === "True"
                if (!isValid) {
                    modalDiv.find('.modal').modal('hide')
                    const newTableRow = `<tr name="${category.CategoryDto.Category.Id}">
        <td>${category.CategoryDto.Category.Id}</td>
        <td>${category.CategoryDto.Category.Title}</td>
        <td>${category.CategoryDto.Category.Description}</td>
        <td>${ısActiveSpan(category.CategoryDto.Category.IsActive)}</td>
        <td>${ısDeleteSpan(category.CategoryDto.Category.IsDeleted)}</td>
        <td>${category.CategoryDto.Category.Note}</td>
        <td>${category.CategoryDto.Category.CreatedByName}</td>
        <td>${converToShortDate(category.CategoryDto.Category.CreatedDate)}</td>
        <td>${category.CategoryDto.Category.ModifiedByName}</td>
        <td>${converToShortDate(category.CategoryDto.Category.ModifiedDate)}</td>
        <td>
            <div class="btn-group">
                <a id="btn-category-delete" class="btn btn-outline-danger btn-sm" data-id="${category.CategoryDto.Category.Id}"><span class=" fas fa-minus-circle"></span> </a>
                <a id="btn-category-update" class="btn btn-outline-warning btn-sm"><span class="fas fa-edit"></span> </a>
            </div>
        </td>
    </tr>`
                    console.log('TableRow', newTableRow)
                    const newTableRowObject = $(newTableRow)
                    console.log(newTableRowObject)
                    newTableRowObject.hide()
                    $('#category-table').append(newTableRowObject)
                    newTableRowObject.fadeIn(1000)
                    toastr.success(`${category.CategoryDto.Messages}`, 'İşlem Başarılı!')
                } else {
                    let summaryText = ""
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text()
                        summaryText = `* ${text}\n`
                    })
                    toastr.warning(summaryText)
                }

            })
        })
    })
    /*POST END HERE */
    /*DELETE START HERE */
    $(document).on('click', '#btn-category-delete', function (event) {
        const id = $(this).attr('data-id')
        const tableRow = $(`[name="${id}"]`)
        const categoryName = tableRow.find('td:eq(1)').text()

        event.preventDefault()
        Swal.fire({
            title: 'Silmek istediğine emin misin?',
            text: `"${categoryName}" adlı kategori silinecektir!`,
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
                    data: { categoryId: id },
                    url: '/Admin/Category/Delete',
                    success: function (data) {
                        const result = jQuery.parseJSON(data)
                        if (result.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                'Kategori Silindi.',
                                'success'
                            )
                            tableRow.fadeOut(2500)
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

    $(function () {
        const modalDiv = $('#category-modal')
        const url = '/Admin/Category/Update/'

        $(document).on('click', '#btn-category-update', function (event) {
            event.preventDefault()
            const id = $(this).attr('data-id')
            $.get(url, { categoryId: id }).done(function (data) {
                console.log("DATA=", data)
                modalDiv.html(data)
                modalDiv.find('.modal').modal('show')

            }).fail(function () {
                toastr.error('Bir Hata Oluştu', 'HATA')
            })
        })

        modalDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault()

            const form = $('#category-update-form')
            const actionUlr = form.attr('action')
            const dataTosend = form.serialize()
            $.post(actionUlr, dataTosend).done(function (data) {

                const categoryUpdate = jQuery.parseJSON(data)
                console.log(categoryUpdate)
                /*validation modal-değişimi*/
                const newFormBody = $('.modal-body', categoryUpdate.CategoryPartial)
                modalDiv.find('.modal-body').replaceWith(newFormBody)
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True'
                if (isValid) {
                    modalDiv.find('.modal').modal('hide')
                    const newTableRow = `<tr name=" ${categoryUpdate.CategoryDto.Category.Id}">
                                    <td>${categoryUpdate.CategoryDto.Category.Id}</td>
                                    <td>${categoryUpdate.CategoryDto.Category.Title}</td>
                                    <td>${categoryUpdate.CategoryDto.Category.Description}</td>
                                    <td>${ısActiveSpan(categoryUpdate.CategoryDto.Category.IsActive)}</td>
                                    <td>${ısDeleteSpan(categoryUpdate.CategoryDto.Category.IsDeleted)}</td>
                                    <td>${categoryUpdate.CategoryDto.Category.Note}</td>
                                    <td>${categoryUpdate.CategoryDto.Category.CreatedByName}</td>
                                    <td>${converToShortDate(categoryUpdate.CategoryDto.Category.CreatedDate)}</td>
                                    <td>${categoryUpdate.CategoryDto.Category.ModifiedByName}</td>
                                    <td>${converToShortDate(categoryUpdate.CategoryDto.Category.ModifiedDate)}</td>
                                    <td>
                                        <div class="btn-group">
                                            <a id="btn-category-delete" class="btn btn-outline-danger btn-sm" data-id="${categoryUpdate.CategoryDto.Category.Id} "><span class=" fas fa-minus-circle"></span> </a>
                                            <a id="btn-category-update" class="btn btn-outline-warning btn-sm"><span class="fas fa-edit"></span> </a>
                                        </div>
                                    </td>
                                </tr>`

                    const newTableRowObject = $(newTableRow)
                    const tableRow = $(`[name="${categoryUpdate.CategoryDto.Category.Id}"]`)
                    newTableRowObject.hide()
                    tableRow.replaceWith(newTableRowObject)
                    newTableRowObject.fadeIn(2500)
                    toastr.success(`${categoryUpdate.CategoryDto.Messages}`, 'İşlem Başarılı!')
                }
                else {
                    /*Toastr Validation*/
                    let summaryText = ""
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text()
                        summaryText = `* ${text}\n`
                    })
                    toastr.warning(summaryText)
                }


            }).fail(function (response) {
                console.log(response)
            })
        })
    })
    /*UPDATE END HERE*/
});