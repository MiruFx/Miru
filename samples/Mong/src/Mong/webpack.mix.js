const mix = require('laravel-mix')

mix.js('resources/js/app.js', 'wwwroot/js')
    .sourceMaps()
    .sass('resources/sass/app.scss', 'wwwroot/css')
    .setPublicPath('wwwroot')
    .version()