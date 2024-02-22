const esbuild = require("esbuild")
const sass = require("esbuild-sass-plugin")
const production = process.argv.indexOf('--production') > -1
const watch = process.argv.indexOf('--watch') > -1

esbuild.build({
    entryPoints: ["resources/css/app.scss", "resources/js/app.js"],
    outdir: "wwwroot",
    bundle: true,
    minify: production,
    // watch: watch,
    metafile: true,
    plugins: [sass.sassPlugin()],
    logLevel: 'info',
})
.catch(() => process.exit(1))