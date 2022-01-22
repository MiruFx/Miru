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

appSettings
    appSettings.yml for any environments
          <ItemGroup>
            <Content Include="appSettings*.yml">
              <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
            </Content>
          </ItemGroup>
-->