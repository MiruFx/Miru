<!-- 
Configuring
    Environments
    appSettings.{environment}.yml
    {App}Options

{App}Options
    {app}/Config/{App}Options.cs
    Program.cs
        services.Configure<AppOptions>(host.Configuration.GetSection("App"))

    ConfigureServices
        .AddSingleton<AppOptions>
-->