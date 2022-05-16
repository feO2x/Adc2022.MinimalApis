using Light.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApis.RealWorldApp.Contacts;
using MinimalApis.RealWorldApp.DataAccess;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureDependencyInjectionContainer(this WebApplicationBuilder builder)
    {
        builder.Host.UseLightInject();
        builder.Services.ConfigureServices();
        return builder;
    }

    private static void ConfigureServices(this IServiceCollection services) =>
        services.AddSwagger()
                .AddCoreServices()
                .AddDataAccess()
                .AddContactsModule()
                .AddAutomaticEndpoints();

    private static IServiceCollection AddCoreServices(this IServiceCollection services) =>
        services.AddSingleton<IValidationContextFactory>(ValidationContextFactory.Instance);
}