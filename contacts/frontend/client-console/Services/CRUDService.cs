using System.Net.Http.Headers;
using System.Xml.Serialization;

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

        // it's a good practice to clear the default headers and add the ones you need

        httpClient.DefaultRequestHeaders.Clear();

        // we can add multiple Accept headers

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.8));

        // make a request

        //var request = new HttpRequestMessage(HttpMethod.Get, "api/contacts");
        //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //var response = await httpClient.SendAsync(request);

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

    public async Task<ContactDetailsDto?> GetContactAsync(int id)
    {
        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

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

    public async Task RunAsync()
    {
        // R(-ead)
        {
            // read all contacts

            Console.WriteLine("GetContacts:\n");

            var contactDtos = await GetContactsAsync();

            foreach (var contactDto in contactDtos)
            {
                Console.WriteLine($"{contactDto.Id} {contactDto.FirstName} {contactDto.LastName} {contactDto.Email}");
            }

            // read a single contact

            Console.WriteLine("\nGetContact:\n");

            var id = 1;

            var contactDetailsDto = await GetContactAsync(id);

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