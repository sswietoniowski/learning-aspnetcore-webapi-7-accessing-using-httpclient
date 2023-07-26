using System.Text;

using Contacts.Client.DTOs;
using Contacts.Client.Helpers;

namespace Contacts.Client.Services;

using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Serialization;

// ReSharper disable once InconsistentNaming - CRUD is an acronym
public class CRUDService : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public CRUDService(IHttpClientFactory httpClientFactory, JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ?? throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
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
            contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, _jsonSerializerOptionsWrapper.Options);
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
        {
            // read all contacts

            Console.WriteLine("\nGetContacts:\n");

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