
import { processPageUpdates } from "./index"
import Rails from "@rails/ujs"

addEventListener("ajax:beforeSend", acceptPageUpdates)
addEventListener("ajax:success", handlePageUpdates)
addEventListener("ajax:error", handlePageUpdates)

const MIME_TYPE = "text/html; page-update"

function acceptPageUpdates(event) {
  const request = event.detail[0];
  
  request.setRequestHeader("Accept", MIME_TYPE)
  request.setRequestHeader("X-Miru-Target", event.target.id)
  
  if (Rails.matches(event.target, "[data-feature]")) {
    request.setRequestHeader("X-Miru-Feature", event.target.getAttribute("data-feature"))
  }
}

function handlePageUpdates(event) {
  const response = event.detail[2]

  event.preventDefault()
  
  processPageUpdates(response.responseText)
}