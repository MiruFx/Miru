import 'bootstrap'

import 'miru-core/dist/miru-rails'

import Rails from '@rails/ujs'

Rails.delegate(document, 'select[data-cascade]', 'ajax:complete', event => {
    let selectId = event.target.getAttribute("data-cascade")
    let select = document.getElementById(selectId)
    select.innerHTML = event.detail[0].responseText
})