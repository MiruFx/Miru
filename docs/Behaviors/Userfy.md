<!-- 
Current Attributes
    
    User
    Permissions
    Company
    Tenant
    Language
    Currency
    Preferences
    ...
-->

# Userfy

Userfy is Miru's extension on top of .NET Identity. Miru creates a layer of simplification for common scenarios.

## Installing

Register `Userfy` in your application's `ConfigureServices`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddMiru<Startup>()
        
        // ... other services
        
        .AddUserfy(cfg => 
        {
            // userfy configurations
        });
}            
```

## Configuring

## Extending

### User.Name

Some applications need to display the User's name instead of its email. 
By default, `Userfy` uses email when registering and display User's information.

#### Migration

To add User.Name in your application, first create a database migration:

```shell
miru make.migration AlterUsersAddName
```

Edit the generated migration with the following:

```csharp
public override void Up()
{
    Alter.Table("Users").AddColumn("Name").AsString(64);
}

public override void Down()
{
    Delete.Column("Name").FromTable("Users");
}
```

#### Entity

Add the `Name` property into your `{app}/Domain/User.cs`:

```csharp
public string Name { get; set; }
```

#### UserEdit Feature

