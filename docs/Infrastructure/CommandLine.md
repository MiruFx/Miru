<!--
Getting Started
    TODO: addconsolables
    TODO: root, miru @app, miru @test, miru @pagetest
Environment
Consolables
    TODO: location
Create
Consolable
    TODO: name
    TODO: arguments
Run
    TODO: logging
Makers
-->

[[toc]]

# Command Line

Miru is command-line friendly offering a lot of facilities, helping to execute tasks that help you build the application or execute your own tasks. It's based on [Oakton](https://jasperfx.github.io/oakton/documentation/).

To see the available commands, in the root of the solution, call ```miru``` or ```miru help```:

![](/CommandLine-List.png)

These tasks are called ```Consolables```. Miru comes with a bunch of them but you can create your own. 

## Environment

To run a task targeting a specific environment, the arguments ```--environment``` or ```-e``` can be used:

```shell
miru config:show -e Test

miru config:show --environment Staging
```

The default environment is ```Development```:

![](/CommandLine-Environment.png)

## Consolables

Tasks are Consolables which are classes that implement ```Miru.Consolables.IConsolable```. In Miru startup, it scans assemblies looking for classes IConsolable.

## Create

Can be created using Miru Makers:

```shell
miru make:consolable Hi
```

## The Consolable

A Consolable has an ```Input``` subclass that can receive arguments from the command line. A Consolable can receive dependencies through constructor.

@[code lang=csharp transcludeWith=#consolable](@/samples/Skeleton/src/Skeleton/Consolables/HiConsolable.cs)

## Running

::: warning
```miru``` command run using the compiled binaries from the solution. In case of changes, is required compile the solution
:::

![](/CommandLine-Run.png)