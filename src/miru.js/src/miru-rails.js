import Rails from '@rails/ujs'
import Turbolinks from 'turbolinks'
import './page_updater/initialize'
import { queue } from "./page_updater/index"
import {Update} from "./page_updater/update"

Rails.start()
Turbolinks.start()

addEventListener("click", event => {
    const element = event.target

    if (Rails.matches(element, '[data-page-update]')) {
        const updatesTargets = element.getAttribute('data-page-update')
        const updates = updatesTargets.split(",").map(item => item.trim())

        updates.forEach(update => {
            const [ type, id ] = update.split("#")
            queue(new Update(type, id))
        })

        event.preventDefault()
    }
});

addEventListener("turbolinks:click", event => {
    if (event.target.getAttribute('href').charAt(0) === '#' || event.target.hasAttribute('data-remote')) {
        return event.preventDefault();
    }
});

