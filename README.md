# Learning ASP.NET Core - WebAPI (.NET 7) Accessing Using HttpClient

This repository contains examples showing how to accessing APIs using HttpClient in .NET 7.

Based on this course [Accessing APIs Using HttpClient in .NET 6](https://app.pluralsight.com/library/courses/dot-net-6-httpclient-using-accessing-apis/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/dot-net-6-httpclient-using-accessing-apis/exercise-files) and [here](https://github.com/KevinDockx/AccessingAPIsWithHttpClientDotNet6).

This repository is continuation/update of [this](https://github.com/sswietoniowski/work-codecool-csharp-httpclient-MyMovies) one.

## Table of Contents

- [Learning ASP.NET Core - WebAPI (.NET 7) Accessing Using HttpClient](#learning-aspnet-core---webapi-net-7-accessing-using-httpclient)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
  - [Understanding Integration with an API Using `HttpClient`](#understanding-integration-with-an-api-using-httpclient)
    - [Strategies for Working with DTO Model Classes](#strategies-for-working-with-dto-model-classes)
    - [Generating DTO Classes](#generating-dto-classes)
      - [Generating DTO Classes from Visual Studio](#generating-dto-classes-from-visual-studio)
      - [Generating DTO Classes with NSwagStudio](#generating-dto-classes-with-nswagstudio)
    - [Tackling Integration with HttpClient](#tackling-integration-with-httpclient)
  - [Handling Common Types of Integration (CRUD)](#handling-common-types-of-integration-crud)
    - [Getting a Resource](#getting-a-resource)
    - [Working with Headers and Content Negotiation](#working-with-headers-and-content-negotiation)
    - [Manipulating Request Headers](#manipulating-request-headers)
    - [Indicating Preference with the Relative Quality Parameter](#indicating-preference-with-the-relative-quality-parameter)
    - [Working with `HttpRequestMessage` Directly](#working-with-httprequestmessage-directly)
    - [Providing Default Values for HttpClient and `JsonSerializerOptions`](#providing-default-values-for-httpclient-and-jsonserializeroptions)
    - [Creating a Resource](#creating-a-resource)
    - [Setting Request Headers](#setting-request-headers)
    - [Inspecting Content Types](#inspecting-content-types)
    - [Updating a Resource](#updating-a-resource)
    - [Deleting a Resource](#deleting-a-resource)
    - [Using Shortcuts](#using-shortcuts)
  - [Summary](#summary)

## Setup

To run API:

```cmd
cd .\contacts\backend\api
dotnet restore
dotnet build
dotnet watch run
```

To run a console client:

```cmd
cd .\contacts\frontend\client-console
dotnet restore
dotnet build
dotnet watch run
```

## Understanding Integration with an API Using `HttpClient`

Whenever we want to communicate with an API, we need a tool for that, one choice is `HttpClient`. HttpClient is built into .NET, and it's a very powerful tool for communicating with APIs.

There are other options:

- [RestSharp](https://restsharp.dev/) [:file_folder:](https://github.com/restsharp/RestSharp) - a simple REST and HTTP API Client for .NET,
- [Refit](https://github.com/reactiveui/refit) - a library heavily inspired by Square's [Retrofit](https://square.github.io/retrofit/) library for Java, and it turns your REST API into a live interface,
- [Flurl](https://flurl.dev/) [:file_folder:](https://github.com/tmenier/Flurl) - a modern, fluent, asynchronous, testable, portable, buzzword-laden URL builder and HTTP client library for .NET.

### Strategies for Working with DTO Model Classes

Shared model project:

- diminishes code duplication, changes only have to be applied in one place,
- useful when you want to deploy the model assembly independently,
- requires control over API and client,
- both must target supported platforms.

Linked files:

- diminishes code duplication, changes only have to be applied in one place,
- model classes are packaged in API and client assemblies,
- requires control over API and client,
- bot must target supported platforms.

What if you don't have control over the API?

- might be built in another technology,
- might be built by another team.

The technology the API is built with shouldn't matter to the client.

### Generating DTO Classes

> Modern-day generation relies on a machine-readable description of the API, and that's where OpenAPI comes in.

To generate DTO classes you've got couple options:

- [`Swagger CodeGen`](https://swagger.io/tools/swagger-codegen/) - requires Java,
- [`NSwagStudio`](https://github.com/RicoSuter/NSwag/wiki/NSwagStudio),
- add connected service option in Visual Studio (it is using NSwag internally).

#### Generating DTO Classes from Visual Studio

If you want to generate DTO classes from Visual Studio, you should follow [this](https://learn.microsoft.com/en-us/visualstudio/azure/overview-connected-services?view=vs-2022) article.

While generated client might help you start, it is not a good idea to use it in production.

#### Generating DTO Classes with NSwagStudio

NSwagStudio is a better option compared to Visual Studio (it's more reliable and flexible).

[Here](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-7.0&tabs=visual-studio#code-generation) you will find a guide on how to generate DTO classes with NSwagStudio.

You can install NSwagStudio like so (provided that you've got Chocolatey installed):

```cmd
choco install nswagstudio
```

Generated DTOs are not perfect, but they are a good starting point.

Example of generated DTOs:

```csharp
// ...

// ReSharper disable CheckNamespace I keep the code generated by NSwagStudio in one file
namespace Contacts.Client.Console.DTOs;

/// <summary>
/// The contact details
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ContactDetailsDto
{
    /// <summary>
    /// The id of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("id")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    /// <summary>
    /// The first name of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("firstName")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The last name of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("lastName")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("fullName")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The email of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("email")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phones of the contact
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("phones")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public System.Collections.Generic.ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
}

// ...
```

### Tackling Integration with HttpClient

> HTTP is a request-response protocol between a client and server. A browser is an HTTP client that send messages and capture responses.

Basic usage of HttpClient:

```csharp
var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://localhost:5001/api/contacts");

response.EnsureSuccessStatusCode();

var content = await response.Content.ReadAsStringAsync();
var contacts = JsonSerializer.Deserialize<List<ContactDto>>(content);
```

## Handling Common Types of Integration (CRUD)

We should know how to perform basic CRUD operations using HttpClient.

### Getting a Resource

An example of getting a _resource_ was already shown in the previous section, but in our case
we must tweak it a little bit to make it work:

```csharp
// R(-ead)

{
    // read all contacts

    Console.WriteLine("GetContacts:\n");

    var contactDtos = await GetContacts();

    foreach (var contactDto in contactDtos)
    {
        Console.WriteLine($"{contactDto.Id} {contactDto.FirstName} {contactDto.LastName} {contactDto.Email}");
    }

    // read a single contact

    Console.WriteLine("\nGetContact:\n");

    var id = 1;

    var contactDetailsDto = await GetContact(id);

    if (contactDetailsDto is not null)
    {
        Console.WriteLine($"{contactDetailsDto.Id} {contactDetailsDto.FirstName} {contactDetailsDto.LastName} {contactDetailsDto.Email}");
    }
    else
    {
        Console.WriteLine($"Contact with id {id} not found");
    }
}

static async Task<List<ContactDto>> GetContacts()
{
    var httpClient = new HttpClient();

    // our API requires Accept: application/json
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var response = await httpClient.GetAsync("https://localhost:5001/api/contacts");

    // we want to make sure that the API responded with 200 OK, if not we throw an exception
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();

    // our API returns JSON in camelCase, we want to deserialize it to PascalCase
    var jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    var contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, jsonSerializerOptions);
    // there is a chance that the API returns null, so we need to check for that
    contactDtos ??= Enumerable.Empty<ContactDto>().ToList();

    return contactDtos;
}

static async Task<ContactDetailsDto?> GetContact(int id)
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var response = await httpClient.GetAsync($"https://localhost:5001/api/contacts/{id}");

    if (response.StatusCode == HttpStatusCode.NotFound)
    {
        return null;
    }

    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();

    // because this class was generated by NSwagStudio, we don't need to worry about the casing, as it is
    // handled by the generated code (attributes)
    var contactDetailsDto = JsonSerializer.Deserialize<ContactDetailsDto>(content);

    return contactDetailsDto;
}
```

While we can create a new instance of `HttpClient` every time we want to make a request, it is not a good idea, as it is a heavy object.
For our simple console application it is not a big deal, but in a real application, especially a web application, it could be a problem.

A better way to do it is to create a single instance of `HttpClient` and reuse it.

Let's change our code to do that.

First add new packages `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.Http`:

```cmd
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Http
```

Then add new directory `Services` and create a new interface `IIntegrationService`:

```csharp
namespace Contacts.Client.Console.Services;

public interface IIntegrationService
{
    Task Run();
}
```

Then create a concrete class `CRUDService`:

```csharp
using Contacts.Client.DTOs;

namespace Contacts.Client.Services;

using System;
using System.Net;
using System.Text.Json;

// ReSharper disable once InconsistentNaming - CRUD is an acronym
public class CRUDService : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CRUDService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<List<ContactDto>> GetContactsAsync()
    {
        // create a named http client

        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        // make a request

        var response = await httpClient.GetAsync("api/contacts");

        // check the response, throw if not successful

        response.EnsureSuccessStatusCode();

        // read the response content

        var content = await response.Content.ReadAsStringAsync();

        // deserialize the response content

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, jsonSerializerOptions);
        contactDtos ??= Enumerable.Empty<ContactDto>().ToList();

        return contactDtos;
    }

    public async Task<ContactDetailsDto?> GetContact(int id)
    {
        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var response = await httpClient.GetAsync($"api/contacts/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var contactDetailsDto = JsonSerializer.Deserialize<ContactDetailsDto>(content);

        return contactDetailsDto;
    }

    public async Task Run()
    {
        // R(-ead)
        {
            // read all contacts

            Console.WriteLine("GetContacts:\n");

            var contactDtos = await GetContacts();

            foreach (var contactDto in contactDtos)
            {
                Console.WriteLine($"{contactDto.Id} {contactDto.FirstName} {contactDto.LastName} {contactDto.Email}");
            }

            // read a single contact

            Console.WriteLine("\nGetContact:\n");

            var id = 1;

            var contactDetailsDto = await GetContact(id);

            if (contactDetailsDto is not null)
            {
                Console.WriteLine($"{contactDetailsDto.Id} {contactDetailsDto.FirstName} {contactDetailsDto.LastName} {contactDetailsDto.Email}");
            }
            else
            {
                Console.WriteLine($"Contact with id {id} not found");
            }
        }
    }
}
```

And finally change `Program.cs` like so:

```csharp
using System.Net.Http.Headers;

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

        // http client

        services.AddHttpClient("ContactsAPIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        // integration service (CRUD)

        services.AddScoped<IIntegrationService, CRUDService>();
    })
    .Build();

try
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Host created.");

    await host.Services.GetRequiredService<IIntegrationService>().Run();
}
catch (Exception generalException)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(generalException, "An exception happened while running the integration service.");
}

await host.RunAsync();
```

**Warning**:

Even though `HttpClient` class implements `IDisposable`, it is not a good idea to dispose it with `using`. More on that [here](https://youtu.be/M-iysvlvOjM),
and for mor info about `IHttpClientFactory` check out [this](https://youtu.be/xI6uMT0bg4I) video.

### Working with Headers and Content Negotiation

_HTTP headers_ allow passing additional information with each request or response.

They are key-value pairs, and they are used to pass information about the request or response.

```text
name: value
name: value1, value2
```

_Request headers_ contain information on the resource to be fetched or about the client itself. They are provided by the client.

Example:

```http
Accept: application/json
Accept: application/xml, text/xml
```

_Response headers_ contain information on the generated response or about the server. They are provided by the server.

Example:

```http
Content-Type: application/json
```

Be as strict as possible!

- for example, setting an `Accept` header (obligatory in RESTful systems) improves reliability.

> _Content negotiation_ the mechanism used for serving different representations of a resource at the same URI,
> so that the user agent can specify which representation is best suited for the user.

Content negotiation is driven by:

- `Accept`,
- `Accept-Encoding`,
- `Accept-Language`,
- `Accept-Charset`.

### Manipulating Request Headers

We already used `DefaultRequestHeaders` property of `HttpClient` to set `Accept` header, but we can also use it to set other headers.

```csharp
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    // we can also set custom headers
    //client.DefaultRequestHeaders.Add("X-MyCustomHeader", "MyCustomValue");
```

Provided that the API we're trying to use is serving multiple content types, we can use `Accept` header to indicate which one we prefer.

Also we should be ready to handle different content types, like so:

```csharp
    public async Task<List<ContactDto>> GetContactsAsync()
    {
        // create a named http client

        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        // it's a good practice to clear the default headers and add the ones you need

        httpClient.DefaultRequestHeaders.Clear();

        // we can add multiple Accept headers

        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

        // make a request

        var response = await httpClient.GetAsync("api/contacts");

        // check the response, throw if not successful

        response.EnsureSuccessStatusCode();

        // read the response content

        var content = await response.Content.ReadAsStringAsync();

        // deserialize the response content

        var contactDtos = new List<ContactDto>();

        // we can use the content type header to determine the deserialization type

        if (response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, jsonSerializerOptions);
        }
        else if (response.Content.Headers.ContentType?.MediaType == "application/xml")
        {
            var xmlSerializer = new XmlSerializer(typeof(List<ContactDto>));
            contactDtos = xmlSerializer.Deserialize(new StringReader(content)) as List<ContactDto>;
        }

        contactDtos ??= Enumerable.Empty<ContactDto>().ToList();

        return contactDtos;
    }
```

### Indicating Preference with the Relative Quality Parameter

Equal preference:

```http
Accept: application/json, application/xml
```

Indicating preference:

```http
Accept: application/json, application/xml;q=0.8
```

We can change our previous code, so it would be our preference to use `application/json` (smaller, less verbose), to do that, we can write it like so:

```csharp
        // we can add multiple Accept headers

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.8));
```

This preference must be supported by the API, otherwise we will get a `406 Not Acceptable` response. ASP.NET Core supports it out of the box.

### Working with `HttpRequestMessage` Directly

Sofar we were using `DefaultRequestHeaders`, while we can do that it would be better if we could be more specific about the headers we want to set and when.

For that we can use `HttpRequestMessage` directly like so:

```csharp
        // make a request

        var request = new HttpRequestMessage(HttpMethod.Get, "api/contacts");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
```

### Providing Default Values for HttpClient and `JsonSerializerOptions`

We already did that while registering `HttpClient` in `Program.cs`:

```csharp
        services.AddHttpClient("ContactsAPIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Clear();
            // defining content type here is not a good idea these days because the preference is to use vendor specific content types
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(30);
        });
```

To define `JsonSerializerOptions` global object that we would be able to use later, we must use a "trick",
first I've added a new directory `Helpers` and in it a new class `JsonSerializerOptionsWrapper` like so:

```csharp
using System.Text.Json;

namespace Contacts.Client.Helpers;

public class JsonSerializerOptionsWrapper
{
    public JsonSerializerOptions Options { get; }

    public JsonSerializerOptionsWrapper()
    {
        Options = new JsonSerializerOptions(
            JsonSerializerDefaults.Web);
        Options.DefaultBufferSize = 10;
    }
}
```

Then we need to register this class as a singleton:

```csharp
        // json serializer options

        services.AddSingleton<JsonSerializerOptionsWrapper>();
```

Now we can inject that object into our service:

```csharp
public class CRUDService : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public CRUDService(IHttpClientFactory httpClientFactory, JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ?? throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
    }

    // ...
```

And use it whenever you need to:

```csharp
        // ...

        if (response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, _jsonSerializerOptionsWrapper.Options);
        }

        // ...
```

An interesting discussion about `JsonSerializerOptions` default settings can be read [here](https://github.com/dotnet/runtime/issues/31094).

Also look [here](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/configure-options?pivots=dotnet-7-0#web-defaults-for-jsonserializeroptions).

### Creating a Resource

To create a new resource we can use this code:

```csharp
    public async Task<ContactForCreationDto> CreateContact(ContactForCreationDto contactForCreationDto)
    {
        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        var content = JsonSerializer.Serialize(contactForCreationDto, _jsonSerializerOptionsWrapper.Options);

        var request = new HttpRequestMessage(HttpMethod.Post, "api/contacts");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(content, Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // we could use PostAsJsonAsync instead of the above like so:
        // var response = await httpClient.PostAsJsonAsync("api/contacts", contactForCreationDto);

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        content = await response.Content.ReadAsStringAsync();

        contactForCreationDto = JsonSerializer.Deserialize<ContactForCreationDto>(content, _jsonSerializerOptionsWrapper.Options)!;

        return contactForCreationDto;
    }
```

and test it like so:

```csharp
    public async Task RunAsync()
    {
        // C(-reate)

        {
            var contactForCreationDto = new ContactForCreationDto("John", "Doe", "jdoe@unknown.com");

            Console.WriteLine("CreateContact:\n");

            contactForCreationDto = await CreateContact(contactForCreationDto);

            Console.WriteLine($"{contactForCreationDto.FirstName} {contactForCreationDto.LastName} {contactForCreationDto.Email}");
        }

        // R(-ead)

        // ...
```

### Setting Request Headers

`HttpClient.DefaultRequestHeaders` is a property that allows us to set default headers for all requests.

`HttpRequestMessage.Headers` is a property that allows us to set headers for a specific request.

`HttpRequestMessage.Content.Headers` is a property that allows us to set headers for a specific request.

### Inspecting Content Types

Use a derived class that matches the content of the message:

- `StringContent`,
- `ObjectContent`,
- `ByteArrayContent`,
- `StreamContent`.

Optimized for their type of content.

### Updating a Resource

Showed during demo.

### Deleting a Resource

Showed during demo.

### Using Shortcuts

Showed during demo.

## Summary

Now you know how to use HttpClient to interact with an API.
