export function createFragment(html) {
  return document.createRange().createContextualFragment(html)
}

export function findElement(id) {
  return document.getElementById(id)
}
