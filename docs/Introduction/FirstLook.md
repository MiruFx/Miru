
# First Overview

This article will show how to create a Solution named ```Supportreon```, with a feature that users can create new Project.

[[toc]]

# Creating New Solution

## New Solution

The best way to create a new solution is using MiruCli:

```shell
MiruCli new Supportreon
```

MiruCli will create a directory with the Solution's skeleton:

```shell
cd Supportreon
tree .
```

![](/CreatingNewSolution-Directories.png)

## Compiling Solution and Resources

The next step is to build the solution

```shell
dotnet build
```

Then, install the npm libraries and run webpack to generate the application js and css (they can take few minutes):

```shell
miru @app npm install
miru @app npm run dev
```

## Database Migration

Run the database migration command to create the database and its tables:

```shell
miru db:migrate
```

## Run The Application

Now, everything is ready to run the application:

```shell
miru @app dotnet run
```

![Run](/CreatingNewSolution-Run.png)

Access the Application:

![Run](/CreatingNewSolution-Home.png)

## Stopping the Application

`miru @app` is a shortcut to run commands with `src\Supportreon` as working directory. 

So, when `miru @app dotnet run` is ran, actually would be the same as `cd src\Supportreon` plus `dotnet run`.

When using `miru @app`, CTRL+C doesn't work to stop the application. Use `Ctrl+Break` instead.

# Creating Features

## New Project

Users can create a new Project where other users can donate money.

### Create the Project entity:

```shell
miru make:entity Project
````

![](/CreatingNewSolution-Make-Project-Entity.png)

Edit `src\Supportreon\Database\SupportreonDbContext.cs` adding a property for Project's DbSet:

```csharp
// Your entities
public DbSet<Project> Projects { get; set; } 
```

### Create Projects Migration:

```shell
miru make:migration CreateProjects
```

![](/CreatingNewSolution-Make-Project-Entity.png)

Edit `src\Supportreon\Database\Migrations\{DATE_TIME}_CreateProjects.cs` changing the column's name:

```csharp
Create.Table("Projects")
```

### Create Project New Feature

```shell
miru make:feature Projects Project New --new
````

![Make Project New](/CreatingNewSolution-Make-Project-New.png)

Edit `src\Supportreon\Features\Projects\_New.js.cshtml` commenting the link to ```ProjectList```:

```razor
@* <a for="@(new ProjectList())">← Projects</a> *@
```

### Manual Testing

Compile the solution, run database migration, and run the application:

```shell
dotnet build
miru db:migrate
miru @app dotnet run
````

Access the url [http://localhost:5000/Projects/New](http://localhost:5000/Projects/New) and try to create a new Project.

### Automated Testing

MiruCli Makers create tests for some items:

![](/CreatingNewSolution-Tests.png)

There are two ways to run the tests:

* Using the dotnet runner: ```dotnet test```
* Using Miru NUnit runner: ```miru @test dotnet run``` for Unit and Integration tests, and ```miru @pagetest dotnet run``` for Page tests through the browser