import { Controller } from "stimulus"

export default class extends Controller {
    connect() {
        let forId = this.element.getAttribute("data-for")
        let input = document.getElementById(forId)
        input.classList.add("is-invalid")
    }
}