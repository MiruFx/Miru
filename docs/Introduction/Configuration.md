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

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()
            .AddSerilogConfig(_ =>
            {
                _.Miru(LogEventLevel.Information);
                _.EntityFrameworkSql(LogEventLevel.Information);
                _.Authentication(LogEventLevel.Information);
            })

            .AddDefaultPipeline<Startup>()

            .AddEfCoreSqlite<SupportreonDbContext>()

            .AddUserfy<User, SupportreonDbContext>(
                cookie: cfg =>
                {
                    cfg.Cookie.Name = App.Name;
                    cfg.Cookie.HttpOnly = true;
                    cfg.ExpireTimeSpan = TimeSpan.FromHours(2);
                    cfg.LoginPath = "/Accounts/Login";
                },
                identity: cfg =>
                {
                    cfg.SignIn.RequireConfirmedAccount = false;
                    cfg.SignIn.RequireConfirmedEmail = false;
                    cfg.SignIn.RequireConfirmedPhoneNumber = false;

                    cfg.Password.RequiredLength = 3;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireLowercase = false;

                    cfg.User.RequireUniqueEmail = true;
                })
            .AddAuthorizationRules<AuthorizationRulesConfig>()
            .AddBelongsToUser<User>()

            .AddMailing(_ =>
            {
                _.EmailDefaults(email => email.From("noreply@Supportreon.com", "Supportreon"));
            })
            .AddSenderStorage()

            .AddQueuing(_ =>
            {
                _.UseLiteDb();
            })
            .AddHangfireServer();
        
        services.AddSession();
        services.AddDistributedMemoryCache();
        services.AddMemoryCache();

        // your app services
    }
}
```

## appSettings.yml

[[toc]]

Miru can read configurations from yml files targeting an environment.

### Directory

The files stay in `/src/{App}` and are named as `appSettings.{Environment}.yml`. Environment is read from ASP.NET Host.

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


