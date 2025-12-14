using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;

namespace Employees.Infrastructure.Logging
{
    public static class LoggerSetup
    {
        public static void ConfigureSerilog(this IServiceCollection services, IHostEnvironment environment)
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("application", environment.ApplicationName)
                        .Enrich.WithProperty("environment", environment.EnvironmentName)
                        .WriteTo.Console(
                                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                                theme: AnsiConsoleTheme.Code)
                        .WriteTo.File($"logs/{environment.ApplicationName}-log-.txt",
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                            shared: true,
                            retainedFileCountLimit: 7
                        )
                        .CreateLogger();

            services.AddSerilog();
        }
    }
}
