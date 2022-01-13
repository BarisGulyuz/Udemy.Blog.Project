$(document).ready(function () {

    /*DATATABLE START HERE */
    const dataTable = $('#users-table').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center mb-3'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Yeni Kullanıcı',
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
    $(function () {
        const modalDiv = $('#user-modal')
        const url = '/Admin/User/Add'

        $('#btn-add').click(function () {
            $.get(url).done(function (data) {
                modalDiv.html(data)
                modalDiv.find(".modal").modal('show')
            })
        })

        //BUG
        modalDiv.on('click', '#btnSave', function (event) {
            event.preventDefault()
            const form = $('#user-add-form')
            const actionUrl = form.attr('action')
            const dataToSend = new FormData(form.get(0))
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: (function (userData) {
                    const user = jQuery.parseJSON(userData)
                    const newFormBody = $('.modal body', user.UserPartial)
                    modalDiv.find('.modal body').replaceWith(newFormBody)
                    const isValid = newFormBody.find('[name="IsValid"]').val() === "False"
                    if (!isValid) {
                        modalDiv.find('.modal').modal('hide')
                        dataTable.row.add([
                            user.UserDto.User.Id,
                            `<td><img src="/images/${user.UserDto.User.Picture}" width="100" height="100" /></td>`,
                            user.UserDto.User.UserName,
                            user.UserDto.User.Email,
                            user.UserDto.User.PhoneNumber,
                            ` <td>
                                    <a id="btn-category-delete" class="btn btn-outline-danger btn-sm" data-id="user.UserDto.User.Id"><span class="fas fa-minus-circle"></span></a>
                                    <a id="btn-category-update" class="btn btn-outline-warning btn-sm" data-id="user.UserDto.User.Id"><span class="fas fa-edit"></span></a>
                                </td>`
                        ]).draw()
                        toastr.success(`${user.UserDto.Messages}`, 'İşlem Başarılı!')
                    } else {

                        toastr.warning('Error')
                    }
                }),
                error: function () {
                    modalDiv.find('.modal').modal('show')
                    toastr.warning('Validasyon hatası')
                }
            })
        })

    })
    /*POST END HERE */
    /*DELETE START HERE */
    $(document).on('click', '#btn-user-delete', function (event) {
        const id = $(this).attr('data-id')
        const tableRow = $(`[name="${id}"]`)
        const userName = tableRow.find('td:eq(2)').text()

        event.preventDefault()
        Swal.fire({
            title: 'Silmek istediğine emin misin?',
            text: `"${userName}" adlı kullanıcı silinecektir!`,
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
                    data: { userId: id },
                    url: '/Admin/User/Delete',
                    success: function (data) {
                        const result = jQuery.parseJSON(data)
                        if (result.ResultStatus === 0) {
                            Swal.fire(
                                'Silindi!',
                                'Kullanıcı Silindi.',
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

    $(function () {
        const modalDiv = $('#user-modal')
        const url = '/Admin/User/Update/'

        $(document).on('click', '#btn-user-update', function (event) {
            event.preventDefault()
            const id = $(this).attr('data-id')
            $.get(url, { userId: id }).done(function (data) {
                modalDiv.html(data)
                modalDiv.find('.modal').modal('show')

            }).fail(function () {
                toastr.error('Bir Hata Oluştu', 'HATA')
            })
        })


        modalDiv.on('click', '#btnUpdate', function (event) {
            event.preventDefault()
            const form = $('#user-update-form')
            const actionUlr = form.attr('action')
            const dataTosend = new FormData(form.get(0))
            $.ajax({
                url: actionUlr,
                type: 'POST',
                data : dataTosend,
                processData: false,
                contentType: false,
                success: (function (data) {

                    const userUpdate = jQuery.parseJSON(data)
            
                    /*validation modal-değişimi*/
                    const newFormBody = $('.modal-body', userUpdate.UserPartial)
                    modalDiv.find('.modal-body').replaceWith(newFormBody)
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True'
                    if (isValid) {
                        const id = userUpdate.UserDto.User.Id
                        const tableRow = $(`[name="${id}"]`)
                        modalDiv.find('.modal').modal('hide')
                        dataTable.row(tableRow).data([
                            userUpdate.UserDto.User.Id,
                            `<td><img src="/images/${userUpdate.UserDto.User.Picture}" width="100" height="100" /></td>`,
                            userUpdate.UserDto.User.UserName,
                            userUpdate.UserDto.User.Email,
                            userUpdate.UserDto.User.PhoneNumber,
                            ` <td>
                                    <a id="btn-category-delete" class="btn btn-outline-danger btn-sm" data-id="userUpdate.UserDto.User.Id"><span class="fas fa-minus-circle"></span></a>
                                    <a id="btn-category-update" class="btn btn-outline-warning btn-sm" data-id="userUpdate.UserDto.User.Id"><span class="fas fa-edit"></span></a>
                                </td>`
                        ])
                        tableRow.attr('name', `${id}`)
                        dataTable.row(tableRow).invalidate()
                        toastr.success(`${userUpdate.UserDto.Messages}`, 'İşlem Başarılı!')
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
                }),
                error: function (err) {
                    alert('Validasyon Hatası')
                }

            })
        })
    })     
})