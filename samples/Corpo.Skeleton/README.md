# Corpo.Skeleton

## Requirements:

* .NET 5
* node.js v14.0 or higher
* npm v6.0 or higher
* [MiruCli 0.5 or higher](https://mirufx.github.io/Introduction/GettingStarted.html#installing)

To check if everything is installed:

```shell
node -v
npm -v
miru --version
```

## First Run:

Open cmd or shell terminal at the solution's root (where the git repository has been cloned):

Install all npm packages and compile webpack with app's javascript and scss files:

```shell
miru app npm install
miru app npm run dev
```

Build .net solution:
```shell
dotnet build
```

Create the database's schema:
```shell
miru db:migrate
```

Now everything is ready for the first run.

## Run Application

Can be run through Visual Studio, Jetbrains Rider, VSCode, or shell:
```shell
miru app dotnet run
```

## Run Tests

Can be run through Visual Studio, Jetbrains Rider, VSCode, or shell.

Run all the tests:

```shell
dotnet test
```

Run only unit/integration tests:

```shell
miru test dotnet test
```

Run only page tests

```shell
miru pagetest dotnet test
```

## Coding

While coding, there is an option to watch changes in the source code and automatically compile the solution and webpack:

```shell
miru watch
```
