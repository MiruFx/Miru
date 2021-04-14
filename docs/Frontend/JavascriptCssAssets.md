<!-- 
JavascriptCssAssets
  npm & webpack with laravel mix
Npm
  package.json
Javascript
  ecmascript 6
  app.js
Css
  sass
  app.sass
Assets
Bundling
  laravel mix
  webpack.mix.js
  versioning
  wwwroot
  TODO: run dev
  TODO: run watch
Consuming
  _layout
  taghelpers
Packages
  Bootstrap
  MiruRails
  Turbolinks
-->

[[toc]]

# Javascript, Css & Assets

To manage frontend assets, Miru comes with [npm](https://www.npmjs.com/) for packages management and [webpack](https://webpack.js.org/) with [Laravel Mix](https://laravel.com/docs/mix) for bundling.

## npm

By default, Miru has `/src/{App}/package.json` with some packages already specified.

To make sure they are installed, run:

```sh
miru app npm install
```

## Resources

Miru comes with `/src/{App}/resources` directory where scripts, stylesheets, and assets should be placed:

![](/JavascriptCssAssets-Resources.png)

### Javascript

Javascript files can be placed in `/src/{App}/resources/js`. ES2015 syntax is supported.

By default, the main file is `/src/{App}/resources/js/app.js` where others modules can be imported:

```js
import 'bootstrap'
import 'miru-core/dist/miru-rails'

import './your-file.js'
```

### Css

Stylesheet files can be placed in `/src/{App}/resources/sass`. Sass and Css are supported.

By default, the main file is `/src/{App}/resources/js/app.sass` where others modules can be imported:

```css
@import 'variables';

@import '~bootstrap/scss/bootstrap';
```

### Assets

Other assets as fonts, images, and others, can also be placed at `/src/{App}/resources` in their respective directories.

## Bundling

Bundling is made by `Webpack` through [Laravel Mix](https://laravel.com/docs/8.x/mix).

The bundling configuration is placed in `/src/{App}/webpack.mix.js`:

By default, bundled files are copied in `/src/{App}/wwwroot`, which is the directory exposed public by ASP.NET Host:

```js
const mix = require('laravel-mix')

mix.js('resources/js/app.js', 'wwwroot/js')
    .sass('resources/sass/app.scss', 'wwwroot/css')
    .setPublicPath('wwwroot')
    .version()
```

### Bundle

To bundle after any change in `resources` directory, run:

```sh
miru app npm run dev
```

You can also keep webpack running watching for changes and bundle automatically:

```sh
miru app npm run watch
```

## Consuming

Once all assets are bundled in `/src/{App}/wwwroot`, they can be referenced in the Views:

```html
<script mix-src="/js/app.js" defer></script>
<link mix-href="/css/app.css" rel="stylesheet" />
```

Note the tags `mix-src` and `mix-href`. They will check if [Laravel Mix Versioning](https://laravel.com/docs/mix#versioning-and-cache-busting) is being used.

## Packages

By default, Miru comes with these packages:

* [Bootstrap](https://getbootstrap.com/) for nice layout
* [Turbo](https://turbo.hotwire.dev/) for faster navigation