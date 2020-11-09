const mix = require('laravel-mix')

/*
 |--------------------------------------------------------------------------
 | Laravel Mix Asset Management
 |--------------------------------------------------------------------------
 |
 | Laravel Mix provides a clean, fluent API for defining some Webpack build steps
 | for your Miru applications. By default, we are compiling the CSS
 | file for the application as well as bundling up all the JS files.
 |
 */

mix.js('resources/js/app.js', 'wwwroot/js')
    .sass('resources/sass/app.scss', 'wwwroot/css')
    .setPublicPath('wwwroot')
    .version()