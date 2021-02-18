import "bootstrap"
import "@hotwired/turbo"
import { Application } from "stimulus"
import { definitionsFromContext } from "stimulus/webpack-helpers"

const application = Application.start()

// const context = require.context("./controllers", true, /\.js$/)
// application.load(definitionsFromContext(context))

import Rails from "@rails/ujs"

Rails.start()

const { delegate, disableElement, enableElement } = Rails

delegate(document, Rails.linkDisableSelector,   "turbo:before-cache", enableElement)
delegate(document, Rails.buttonDisableSelector, "turbo:before-cache", enableElement)
delegate(document, Rails.buttonDisableSelector, "turbo:submit-end", enableElement)

delegate(document, Rails.formSubmitSelector, "turbo:submit-start", disableElement)
delegate(document, Rails.formSubmitSelector, "turbo:submit-end", enableElement)
delegate(document, Rails.formSubmitSelector, "turbo:before-cache", enableElement)

