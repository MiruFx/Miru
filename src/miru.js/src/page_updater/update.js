import * as commands from "./commands"
import { findElement } from "./util"

export class Update {
  constructor(type, id, content) {
    this.type = type
    this.id = id
    this.content = content
  }

  perform() {
    const command = commands[this.type]
    if (!command) return

    const element = findElement(this.id)
    if (!element) return

    command(element, this.content)
  }
}
