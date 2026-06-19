using SageTestService;
using SageTestService.Database;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

//var builder = Host.CreateApplicationBuilder(args);
var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "sagetest.log");
Log.Logger = new LoggerConfiguration()
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args)
    .UseWindowsService()   // critical for SCM integration
    .ConfigureServices((context, services) =>
    {
        services.AddSerilog();
        services.AddScoped<SageDBAccess>();
        services.AddHostedService<Worker>();

    });

var host = builder.Build();

host.Run();
