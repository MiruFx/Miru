<!--
Configuration
TODO: Command Line args
TODO: Environment variables
Config.yml
  TODO: same as appconfig.json
-->

# Configuration

## Services

Configure the services dependency in Startup.cs

@[code lang=csharp transcludeWith=#startup](@/samples/Mong/src/Mong/Startup.cs)

## Config.yml

[[toc]]

Miru can read configurations from yml files targeting an environment.

### Directory

The files stay in `/config` and are named as `Config.{Environment}.yml`. Environment is read from ASP.NET Host.

![](/Config.yml-Folder.png)

### File

This is an example of a Config.yml

```yml
Database:
  ConnectionString: "DataSource={{ db_dir }}Mong_dev.db"
  
Mailing:
  AppUrl: http://localhost:5000
  Smtp:
    Host: smtp.mailtrap.io
    Port: 25
    UserName: username
    Password: password
```


