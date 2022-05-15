using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureDependencyInjectionContainer(this WebApplicationBuilder builder)
    {
        builder.Host.UseLightInject();
        builder.Services.ConfigureServices(builder.Configuration);
        return builder;
    }

    private static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwagger();
    }
}