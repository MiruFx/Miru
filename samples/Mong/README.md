# Mong

A sample to show some [Miru Framework](https://mirufx.github.io)'s features. The domain is a mobile top-up portal.

## Getting Started

Requirements are: .NET 5, MiruCli, node.js, and npm.

### Install MiruCli

[How to install MiruCli](https://mirufx.github.io/Introduction/GettingStarted.html#installing).

### Clone and Build

* Clone the repository
* Open cmd or shell at /samples/Mong directory
* Build the solution
```
dotnet build
```
* Install npm packages and bundle javascript/css packages (can take some minutes)
```
miru @app npm install
miru @app npm run dev
```

## Run the Application

* Create database running the database migration
```
miru db:migrate
```

* Run the Application
```
miru @app dotnet run
```

### Run Tests

To run all tests:

```
dotnet test
```

To run only Mong.Tests:

```
miru @test dotnet run
```

To run only Mong.PageTests (requires Chrome on Windows or Firefox on Linux/BSD):

```
miru @pagetest dotnet run
``` 


