using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore;
using Motto.API;
using System.Net;
using Serilog;
using Motto.API.Infrastructure;
using Motto.API.Extensions;

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Constants.AppName);
    var host = CreateHostBuilder(configuration, args).Build();

    Log.Information("Applying migrations ({ApplicationContext})...", Constants.AppName);
    host.MigrateDbContext<MottoContext>((context, services) =>
    {
        var env = services.GetRequiredService<IWebHostEnvironment>();
        var logger = services.GetRequiredService<ILogger<MottoContextSeed>>();

        new MottoContextSeed().SeedAsync(context, logger).Wait();
    });

    Log.Information("Starting web host ({ApplicationContext})...", Constants.AppName);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Constants.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
    Host.CreateDefaultBuilder(args).UseSerilog().
    ConfigureWebHostDefaults(webBuilder =>
          webBuilder
          .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
          .CaptureStartupErrors(false)
          .UseStartup<Startup>()
          .UseContentRoot(Directory.GetCurrentDirectory()));


Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Constants.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Build();

    if (config.GetValue<bool>("UseVault", false))
    {
        TokenCredential credential = new ClientSecretCredential(
            config["Vault:TenantId"],
            config["Vault:ClientId"],
            config["Vault:ClientSecret"]);
        //builder.AddAzureKeyVault(new Uri($"https://{config["Vault:Name"]}.vault.azure.net/"), credential);        
    }

    return builder.Build();
}