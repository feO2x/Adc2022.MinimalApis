using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MinimalApis.RealWorldApp.Contacts.GetContactDetails;
using MinimalApis.RealWorldApp.Contacts.GetContacts;
using MinimalApis.RealWorldApp.Heartbeat;
using Serilog;

namespace MinimalApis.RealWorldApp.Infrastructure;

public static class HttpPipeline
{
    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsAndHstsIfNecessary();
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseSwaggerAndSwaggerUi();
        return app.MapEndpoints();
    }

    private static WebApplication MapEndpoints(this WebApplication app) =>
        app.MapHeartbeatEndpoint()
           .MapGetContacts()
           .MapGetContactDetails();

    private static void UseHttpsAndHstsIfNecessary(this WebApplication app)
    {
        var httpMode = app.Configuration.GetValue("httpMode", HttpMode.UseHsts);
        switch (httpMode)
        {
            case HttpMode.UseHttpsRedirection:
                app.UseHttpsRedirection();
                break;
            case HttpMode.UseHsts:
                app.UseHsts()
                   .UseHttpsRedirection();
                break;
        }
    }
}

public enum HttpMode
{
    AllowHttp,
    UseHttpsRedirection,
    UseHsts
}