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

#### Generating DTO Classes from Visual Studio

Showed during demo.

#### Generating DTO Classes with NSwagStudio

Showed during demo.

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

An example of getting a _resource_ was already shown in the previous section:

```csharp
var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://localhost:5001/api/contacts");

response.EnsureSuccessStatusCode();

var content = await response.Content.ReadAsStringAsync();
var contacts = JsonSerializer.Deserialize<List<ContactDto>>(content);
```

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

Showed during demo.

### Indicating Preference with the Relative Quality Parameter

Equal preference:

```http
Accept: application/json, application/xml
```

Indicating preference:

```http
Accept: application/json, application/xml;q=0.8
```

Showed during demo.

### Working with `HttpRequestMessage` Directly

Showed during demo.

### Providing Default Values for HttpClient and `JsonSerializerOptions`

Showed during demo.

### Creating a Resource

Showed during demo.

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
