import { Controller } from "@hotwired/stimulus"

// TODO: rename to form_input_validation_controller
export default class extends Controller {
    connect() {
        let forId = this.element.getAttribute("data-for")
        let input = document.getElementById(forId)
        input.classList.add("is-invalid")
    }
}
