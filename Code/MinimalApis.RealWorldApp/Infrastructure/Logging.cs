using System;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class Logging
{
    private static bool WasLoggerInitialized { get; set; }

    private static LoggingLevelSwitch LoggingLevelSwitch { get; } = new ();

    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        var logFilePath = builder.Configuration["logging:logFilePath"];
        if (logFilePath.IsNullOrWhiteSpace())
            throw new InvalidConfigurationException("The log file path is not configured in appsettings.json");

        logFilePath = Environment.ExpandEnvironmentVariables(logFilePath);
        var logger = CreateLogger(logFilePath);
        Log.Logger = logger;
        builder.Host.UseSerilog(logger);
        WasLoggerInitialized = true;
        builder.Configuration.TryUpdateLoggingLevelSwitchFromConfiguration();
        builder.Services.AddSingleton(LoggingLevelSwitch);
        return builder;
    }

    private static ILogger CreateLogger(string logFilePath) =>
        new LoggerConfiguration().MinimumLevel.ControlledBy(LoggingLevelSwitch)
                                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                                 .WriteTo.Console()
                                 .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                                 .CreateLogger();

    public static IConfiguration TryUpdateLoggingLevelSwitchFromConfiguration(this IConfiguration configuration)
    {
        configuration.MustNotBeNull(nameof(configuration));

        if (Enum.TryParse(configuration["logLevel"], true, out LogEventLevel parsedLevel))
            LoggingLevelSwitch.MinimumLevel = parsedLevel;

        return configuration;
    }

    public static ILogger GetEmergencyLogger() =>
        WasLoggerInitialized ?
            Log.Logger :
            new LoggerConfiguration().WriteTo.File("startup-error.log")
                                     .WriteTo.Console()
                                     .CreateLogger();
}