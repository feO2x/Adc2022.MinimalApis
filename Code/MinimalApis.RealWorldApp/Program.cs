using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using MinimalApis.RealWorldApp.DataAccess;
using MinimalApis.RealWorldApp.Infrastructure;

namespace MinimalApis.RealWorldApp;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            var app = WebApplication.CreateBuilder(args)
                                    .UseSerilog()
                                    .ConfigureDependencyInjectionContainer()
                                    .Build()
                                    .ConfigureHttpPipeline();
            await app.MigrateDatabaseAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception exception)
        {
            var logger = Logging.GetEmergencyLogger();
            logger.Fatal(exception, "Could not start ASP.NET Core Web Host");
            return -1;
        }
    }
}