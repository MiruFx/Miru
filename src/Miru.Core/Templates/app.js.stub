import "bootstrap"
import "@hotwired/turbo"

import { Application } from "@hotwired/stimulus"
import { definitionsFromContext } from "@hotwired/stimulus-webpack-helpers"

window.Stimulus = Application.start()
const context = require.context("./controllers", true, /\.js$/)
Stimulus.load(definitionsFromContext(context))

import Rails from "@rails/ujs"

Rails.start()

const { delegate, disableElement, enableElement } = Rails

delegate(document, Rails.linkDisableSelector,   "turbo:before-cache", enableElement)
delegate(document, Rails.buttonDisableSelector, "turbo:before-cache", enableElement)
delegate(document, Rails.buttonDisableSelector, "turbo:submit-end", enableElement)

delegate(document, Rails.formSubmitSelector, "turbo:submit-start", disableElement)
delegate(document, Rails.formSubmitSelector, "turbo:submit-end", enableElement)
delegate(document, Rails.formSubmitSelector, "turbo:before-cache", enableElement)

