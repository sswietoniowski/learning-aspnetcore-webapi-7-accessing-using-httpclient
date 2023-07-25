# Learning ASP.NET Core - WebAPI (.NET 7) Accessing Using HttpClient

This repository contains examples showing how to accessing APIs using HttpClient in .NET 7.

Based on this course [Accessing APIs Using HttpClient in .NET 6](https://app.pluralsight.com/library/courses/dot-net-6-httpclient-using-accessing-apis/table-of-contents).

Original course materials can be found [here](https://app.pluralsight.com/library/courses/dot-net-6-httpclient-using-accessing-apis/exercise-files) and [here](https://github.com/KevinDockx/AccessingAPIsWithHttpClientDotNet6).

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

To run client:

```cmd

```

## Understanding Integration with an API Using `HttpClient`

### Strategies for Working with DTO Model Classes

### Generating DTO Classes

#### Generating DTO Classes from Visual Studio

#### Generating DTO Classes with NSwagStudio

### Tackling Integration with HttpClient

## Handling Common Types of Integration (CRUD)

### Getting a Resource

### Working with Headers and Content Negotiation

### Manipulating Request Headers

### Indicating Preference with the Relative Quality Parameter

### Working with `HttpRequestMessage` Directly

### Providing Default Values for HttpClient and `JsonSerializerOptions`

### Creating a Resource

### Setting Request Headers

### Inspecting Content Types

### Updating a Resource

### Deleting a Resource

### Using Shortcuts

## Summary

Now you know how to use HttpClient to interact with an API.
