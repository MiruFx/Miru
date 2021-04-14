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
      <img src="https://img.shields.io/nuget/vpre/miru.svg" alt="nuget" height="20">
  </a>  
</p>

::: warning
Miru is at an early stage of development. It can change a lot before 1.0. **It's not recommended to use it in real projects yet**.

This documentation is a draft. It will be filled with more information as Miru goes towards version 1.0.
:::

[[toc]]

## What Miru is?

Miru is an open-source opinionated framework for developing .NET 5 Web Applications. It bundles a lot of great libraries and offers conventions and convenient ways of building applications.

Take a look into the sample [Supportreon](https://github.com/MiruFx/Miru/tree/master/samples/Supportreon)

## Installing

### Requirements

* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) or higher
* [Node.js](https://nodejs.org/en/)
* [npm](https://www.npmjs.com/get-npm)

### Install

In any directory, run:

```
dotnet tool install Miru.Cli -g
```

Check the installed version using:

```
miru --version
```

### Update

```
dotnet tool update Miru.Cli -g
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