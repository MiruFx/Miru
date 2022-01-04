<!-- 
intro

Registry
    .AddTimeStamps()

Decorate ITimeStamped

Database
    New Tables
        .WithTimeStamps
    Alter Tables
        .AsDateTime ...

-->

# TimeStamps

TimeStamps' behavior sets automatically the `CreatedAt` and `UpdatedAt` DateTime properties from an `Entity` when saving on `EntityFramework`.

```csharp
// arrange
var post = new Post();

// act
_.Save(post);

// assert
post.DumpToConsole();

// Post: 
//  CreatedAt: 2021-12-29T15:45:44.8259326+01:00
//  UpdatedAt: 2021-12-29T15:45:44.8261866+01:00
//  Id: 1
```

## Adding Into Your Project

### Register

Add `AddTimeStamps` into your `ConfigureServices`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddMiru<Startup>()
        
        // ... other services
        
        .AddTimeStamps();
}            
```

### Decorate Your Entities

Decorate with `ITimeStamps` the Entities you want to have time stamps:

```csharp
public class Post : Entity, ITimeStamped
{
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### Migration For New Columns 

If you are creating a new table, you can use `WithTimeStamps()`:

```shell
miru make.migration CreateProducts
```

```csharp
[Migration(202112291433)]
public class CreateProducts : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Products")
            .WithId()
            .WithColumn("Name").AsString(64)
            .WithTimeStamps();
    }
}
```

If your tables already exists, you can create a migration to alter the table adding the new columns:

```shell
miru make.migration AlterProductsAddTimeStamps
```

```csharp
[Migration(202112291519)]
public class AlterProductsAddTimeStamps : Migration
{
    public override void Up()
    {
        Alter.Table("Products")
            .AddColumn("CreatedAt").AsDateTime()
            .AddColumn("UpdatedAt").AsDateTime();
    }

    public override void Down()
    {
        Delete.Column("CreatedAt").FromTable("Products");
        Delete.Column("UpdatedAt").FromTable("Products");
    }
}
```