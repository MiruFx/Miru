<!-- 
Introduction
  fluentmigrator
  migration, runner
  TODO: addmigrations
Create
Migration
    create table
        WithId
        WithTimeStamps
        AsForeignKeyReference
    changing columns
    TODO: Naming
        Create table
        Alter table
        Alter many tables
        Rename table
    TODO: Existing database
        create a new database, for example for testing or staging environment
        use a tool to export sql
        add sql into the solution
        create migration importing sql file
Running
    db.migrate
    db.rollback
    db.reset
    db.drop.schema
-->

[[toc]]

# Migrations

Miru Migrations is based on [FluentMigrator](https://fluentmigrator.github.io/)

Migrations are a convenient way to alter your database schema over time in a consistent and easy way.

## Create

Can be created using Miru Makers:

```shell
miru make:migration CreateWithdraws
```

Migrations can be located at ```/src/App/Database/Migrations```.

It's possible to pass with table is being manipulated:

```shell
miru make:migration CreateWithdraws --table Withdraws
```

## Migration

Migration is a class that sets a version of the schema change and the changes:

```csharp
[Migration(202010202107)]
public class CreateWithdraws : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("TableName")
            .WithColumn("Id").AsId()
            .WithColumn("Name").AsString(64);
    }
}
```

More details can be seen at [FluentMigrator's documentation](https://fluentmigrator.github.io/articles/migration-example.html)

## Running

Miru comes with commands to be called from command-line. Some of them manage migrations.

### Migrate Up

Execute migrations that haven't been executed yet.

```shell
miru db:migrate
```

![](/Migrations-Up.png)

### Rollback

Rollback migrations to the last version.

```shell
miru db:rollback
```

![](/Migrations-Rollback.png)

### Reset

Reset database rolling back all and executing all migrations again.

```shell
miru db:reset
```

### Environment

By default, migration commands run in ```Development``` environment. To run in a different one, use ```-e``` or ```--environment``` arguments:

```shell
miru db:reset -e Test

miru db:migrate --environment Staging
```