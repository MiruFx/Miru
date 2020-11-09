export function append(element, content) {
  element.append(content)
}

export function prepend(element, content) {
  element.prepend(content)
}

export function replace(element, content) {
  element.replaceWith(content)
}

export function update(element, content) {
  element.innerHTML = ""
  element.append(content)
}

export function clear(element) {
  element.innerHTML = ""
}

export function remove(element) {
  element.remove()
}

export function hide(element) {
  element.setAttribute("hidden", "hidden")
}

export function show(element) {
  element.removeAttribute("hidden")
}

export function focus(element) {
  element.focus()
}