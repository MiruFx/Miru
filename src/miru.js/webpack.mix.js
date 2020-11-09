let mix = require('laravel-mix');

mix
    .js('src/miru-rails.js', 'dist/miru-rails.js')
    .js('src/miru-rails.js', 'dist/miru-up.js')
    .setPublicPath('dist');