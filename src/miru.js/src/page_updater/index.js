import { extractUpdates } from "./templates"

const updates = []
let animationFrame

export function processPageUpdates(html) {
  extractUpdates(html).forEach(queue)
}

export function queue(update) {
  updates.push(update)
  scheduleRender()
}

function scheduleRender() {
  if (animationFrame) return
  animationFrame = requestAnimationFrame(() => {
    animationFrame = null
    while (updates.length) updates.shift().perform()
  })
}
