// bundle libraries
import "bootstrap"
import "@hotwired/turbo"

import { Application } from "@hotwired/stimulus"
import { definitionsFromContext } from "@hotwired/stimulus-webpack-helpers"

// load controllers
const application = Application.start()
const context = require.context("./controllers", true, /\.js$/)
application.load(definitionsFromContext(context))
window.Stimulus = application

// try to fix turbo's weird scrolling behavior when transition to other page on firefox 
document.addEventListener(`turbo:load`, () => {
    document.documentElement.style.scrollBehavior = `smooth`
})

document.addEventListener(`turbo:before-visit`, () => {
    document.documentElement.style.scrollBehavior = `unset`
})