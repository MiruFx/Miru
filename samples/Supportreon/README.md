# Supportreon

A sample to show some [Miru Framework](https://mirufx.github.io)'s features. The domain is a simple funding platform (like Patreon).

## Getting Started

### Install MiruCli

[How to install MiruCli](https://mirufx.github.io/Introduction/GettingStarted.html#installing).

### Clone and Build

* Clone the repository
* Open cmd or shell in the cloned repository folder
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

To run only Supportreon.Tests:

```
miru @test dotnet run
```

To run only Supportreon.PageTests (requires Chrome):

```
miru @pagetest dotnet run
``` 


