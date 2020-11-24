<!-- 
What Miru is?
Overview
  samples
  TODO: video
TODO: Update packages
  TODO: Update Selenium
TODO: References
TODO: Roadmap and Vision link
-->
# Getting Started

<p>
  <a href="https://www.nuget.org/packages/Miru/">
      <img src="https://badgen.net/nuget/v/Miru/latest" alt="nuget" height="20">
  </a>
  <a href="https://f.feedz.io/miru/miru/nuget/index.json">
      <img src="https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fmiru%2Fmiru%2Fshield%2FMiru%2Flatest&label=dev" alt="dev">
  </a>    
</p>

::: warning
Miru is at an early stage of development. It can change a lot before 1.0. **It's not recommended to use it in real projects yet**.

This documentation is a draft. It will be filled with more information as Miru goes towards version 1.0.
:::

[[toc]]

## What Miru is?

Miru is an open-source opinionated framework for developing .NET 5 Web Applications. It bundles a lot of great libraries and offers conventions and convenient ways of building applications.

For now, the two ways to check how it looks like are:

* Having a [First Look](/Introduction/FirstLook.html) developing a new Solution.
* Taking a look into the samples:
  * [Supportreon](https://github.com/joaofx/Supportreon)
  * [Mong](https://github.com/mirufx/miru/tree/master/samples/Mong)
  * [SelfImprov](https://github.com/mirufx/miru/tree/master/samples/SelfImprov)

## Installing

### Requirements

* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) or higher
* [Node.js](https://nodejs.org/en/)
* [npm](https://www.npmjs.com/get-npm)

### Install

In any directory, run:

```
dotnet tool install MiruCli -g
```

And then:

```
MiruCli setup
```

The output should be:

```
Miru setup successfully. From now use the command anywhere: miru

Good luck!
```

Check the installed version using:

```
miru --version
```

### Update

```
dotnet tool update MiruCli -g

MiruCli setup
```

<!--
Miru is composed by:

* MiruCli: A global .net tool for creating new solutions and executing Miru tasks
* Miru Core: The main library
* Miru Extensions: Other libraries that add specific functionality to the application 

## Dependencies

Some of the amazing libraries and frameworks Miru uses:

* .NET 5
* ASP.NET MVC
* MediatR, 
* EFCore 
* FluentMigrator
* FluentValidation
* HtmlTags
* FluentEmail
* Serilog
* Hangfire
* Bootstrap
* Laravel Mix
* Turbolinks
* Rails-ujs
-->