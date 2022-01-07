function converToShortDate(dateString) {
    const shorDate = new Date(dateString).toLocaleDateString('en-US')
    return shorDate
}

function ısActiveSpan(ısActive) {
    let span = ``
    if (ısActive == true) {
        span = `<span class="badge badge-success">Aktif</span>`
    }
    if (ısActive == false) {
        span = `<span class="badge badge-danger">Pasif</span>`
    }
    return span
}

function ısDeleteSpan(ısDelete) {
    let span = ``
    if (ısDelete == true) {
        span = `<span class="badge badge-danger">Silinmiş</span>`
    }
    if (ısDelete == false) {
        span = `<span class="badge badge-success">Silinmemiş</span>`
    }
    return span
}