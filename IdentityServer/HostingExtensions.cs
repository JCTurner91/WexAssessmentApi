// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Majority of IdentityServer code pulled from the QuickStart guide in the official Duende IdentityServer documentation.
// QuickGuide found here: https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/

using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment, if you want to add an MVC-based UI
        // builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer()
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // uncomment if you want to add a UI
        //app.UseStaticFiles();
        //app.UseRouting();

        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseIdentityServer();

        // uncomment, if you want to add a UI
        //app.UseAuthorization();
        //app.MapRazorPages().RequireAuthorization();

        return app;
    }
}