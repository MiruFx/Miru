<!-- 
Introduction
  efcore
  support: sqlserver, sqlite
Configuration
    addentityframework
    connectionstring
DbContext
    location
Helpers
    TODO: byid, byidorfail, etc
    WhereWhen
-->

[[toc]]

# Entity Framework

Miru uses [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) to persist entities.

Although EFCore supports many database, at the moment Miru supports these:

* SqlServer
* Sqlite

Miru has database facilities in other areas, such as Migrations and Tests. That's why few databases are supported.

## Configuration

### Services

To add EFCore with SqlServer:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMiru<Startup>()
        .AddEfCoreSqlServer<SupportreonDbContext>()
}
```

To add EFCore with SqlSqite:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMiru<Startup>()
        .AddEfCoreSqlite<SupportreonDbContext>()
}
```

### Connection String

Set in your `Config.{Environment}.yml`:

```yml
Database:
  ConnectionString: "DataSource={{ db_dir }}App_dev.db"
```

### DbContext

By default, the DbContext is located at `/src/{App}/Database/`