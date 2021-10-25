import { Controller } from "@hotwired/stimulus"

export default class extends Controller {
    connect() {
        if (this.element.hidden === false && this.element.hasAttribute("data-no-scroll") === false) {

            let headerOffset = 30
            let elementPosition = this.element.getBoundingClientRect().top
            let offsetPosition = elementPosition - headerOffset

            window.scrollTo({
                top: offsetPosition,
                behavior: "smooth"
            })
        }
    }
}
