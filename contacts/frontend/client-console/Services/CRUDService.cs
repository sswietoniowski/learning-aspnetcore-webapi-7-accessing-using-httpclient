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
        var httpClientName = "ContactsAPIClient";
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var response = await httpClient.GetAsync("api/contacts");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var contactDtos = JsonSerializer.Deserialize<List<ContactDto>>(content, jsonSerializerOptions);
        contactDtos ??= Enumerable.Empty<ContactDto>().ToList();

        return contactDtos;
    }

    public async Task<ContactDetailsDto?> GetContactAsync(int id)
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