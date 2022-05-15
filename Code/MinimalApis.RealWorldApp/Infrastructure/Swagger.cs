using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class Swagger
{
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                 {
                     options.SwaggerDoc("v1", new OpenApiInfo
                     {
                         Title = "ADC 2022 Minimal API service",
                         Version = "v1",
                         Description = "An example Minimal API web app for my corresponding talk at the Advanced Developers Conference 2022 in Regensburg, Germany.",
                         Contact = new OpenApiContact
                         {
                             Name = "Kenny Pflug",
                             Email = "kenny.pflug@live.de"
                         }
                     });
                     var assembly = typeof(Swagger).Assembly;
                     var xmlFileName = $"{assembly.GetName().Name}.xml";
                     var xmlFilePath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, xmlFileName);
                     options.IncludeXmlComments(xmlFilePath);
                 });

    public static WebApplication UseSwaggerAndSwaggerUi(this WebApplication app)
    {
        app.UseSwagger()
           .UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ADC 2022 Minimal API Example"));
        return app;
    }
}