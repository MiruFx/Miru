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
Miru will be in alpha state while version is 0.*

**It's not recommended to use it in production yet**.

This documentation is a draft. It will be filled with more information as Miru goes towards version 1.0 in the end of 2022.
:::

[[toc]]

## Installing

Miru has two major components:

* `Miru.dll`: the library to reference in your project
* `Miru.Cli`: the shell client to support your coding 

### Requirements

* [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) or higher
* [Node.js](https://nodejs.org/en/)
* [npm](https://www.npmjs.com/get-npm)

### Installing Miru.Cli

Install Miru.Cli. At any directory, run:

```
dotnet tool install Miru.Cli -g
```

Check the installed version using:

```
miru --version
```

### Updating Miru.Cli

```
dotnet tool update Miru.Cli -g
```

## Creating a new solution

Using shell, go to your projects directory and run:

```
miru new SolutionName
```

It will create the basic directory structure named `SolutionName`

## Preparing solution to run

Using shell, go to `SolutionName` directory.

You will need to install npm libraries, bundle javascript and css, compile .net solution, run the db migration:

```shell
miru app npm install
miru app npm run dev
dotnet build
miru storage.link
miru db.migrate
```

Now you are ready to run the application:

```shell
miru app dotnet run
```

## Dependencies

Some of the amazing libraries and frameworks Miru uses:

* .NET 6
* ASP.NET MVC Core
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
* Hotwire Turbo
* Hotwire Stimulus.js