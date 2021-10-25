import { Controller } from "@hotwired/stimulus"

export default class extends Controller {

    connect() {
        this.element.addEventListener('submit', this.hideValidationMessages);
        this.element.addEventListener('submit', this.hideFormSummary);
        this.element.addEventListener('turbo:submit-start', this.setHeaders);
    }

    setHeaders(e) {
        e.detail.formSubmission.fetchRequest.fetchOptions.headers["turbo-form-id"] = e.target.id
        e.detail.formSubmission.fetchRequest.fetchOptions.headers["turbo-form-summary-id"] = e.target.getAttribute("data-form-summary")
    }
    
    hideFormSummary(e) {
        let formSummaryId = e.target.getAttribute("data-form-summary")
        let formSummary = document.getElementById(formSummaryId)
        if (formSummary)
            formSummary.hidden = true
    }
    
    hideValidationMessages(e) {
        let validations = e.target.querySelectorAll("[data-controller='validation-message']")
        validations.forEach(val => {
            val.hidden = true

            let forId = val.getAttribute("data-for")
            let input = document.getElementById(forId)
            input.classList.remove("is-invalid")
        });
    }
}
