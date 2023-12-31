﻿using System.Net.Http.Headers;

using Contacts.Client.Helpers;
using Contacts.Client.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// create host

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        // register services for DI

        // logging

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        // json serializer options

        services.AddSingleton<JsonSerializerOptionsWrapper>();

        // http client

        services.AddHttpClient("ContactsAPIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Clear();
            // defining content type here is not a good idea these days because the preference is to use vendor specific content types
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // integration service (CRUD)

        services.AddScoped<IIntegrationService, CRUDService>();
    })
    .Build();

try
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Host created.");

    await host.Services.GetRequiredService<IIntegrationService>().RunAsync();
}
catch (Exception generalException)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(generalException, "An exception happened while running the integration service.");
}

await host.RunAsync();
