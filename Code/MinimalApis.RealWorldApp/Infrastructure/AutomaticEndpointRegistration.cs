using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class AutomaticEndpointRegistration
{
    public static IServiceCollection AddAutomaticEndpoints(this IServiceCollection services)
    {
        var types = typeof(AutomaticEndpointRegistration).Assembly
                                                         .GetExportedTypes();
        var minimalApiEndpointType = typeof(IMinimalApiEndpoint);
        foreach (var type in types)
        {
            if (!type.Implements(minimalApiEndpointType))
                continue;

            services.Add(new ServiceDescriptor(minimalApiEndpointType, type, ServiceLifetime.Transient));
        }

        return services;
    }

    public static WebApplication AutomaticallyMapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IMinimalApiEndpoint>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}

public interface IMinimalApiEndpoint
{
    void MapEndpoint(WebApplication app);
}