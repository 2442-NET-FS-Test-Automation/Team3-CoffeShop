import { spawn } from "node:child_process"
import { fileURLToPath } from "node:url"

const [command, ...args] = process.argv.slice(2)
const cypressCli = fileURLToPath(new URL("../node_modules/cypress/bin/cypress", import.meta.url))

delete process.env.ELECTRON_RUN_AS_NODE

const child = spawn(process.execPath, [cypressCli, command, ...args], {
    env: process.env,
    stdio: "inherit",
})

child.on("exit", (code) => process.exit(code ?? 1))
