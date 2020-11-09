import { Update } from "./update"
import { createFragment } from "./util"

const ATTRIBUTE = "data-page-update"
const SELECTOR = `template[${ATTRIBUTE}]`

export function extractUpdates(html) {
  return Array.from(extractTemplates(html), createUpdate)
}

export function createUpdate(template) {
  const [ type, id ] = template.getAttribute(ATTRIBUTE).split("#")
  return new Update(type, id, template.content)
}

function extractTemplates(html) {
  return createFragment(html).querySelectorAll(SELECTOR)
}


