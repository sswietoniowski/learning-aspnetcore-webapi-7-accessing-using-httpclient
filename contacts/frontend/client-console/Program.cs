using System.Net.Http.Headers;

using Contacts.Client.Console.DTOs;
using System.Text.Json;

// R(-ead)

{
    Console.WriteLine("GetContacts:\n");

    var contactDtos = await GetContacts();

    foreach (var contactDto in contactDtos)
    {
        Console.WriteLine($"{contactDto.Id} {contactDto.FirstName} {contactDto.LastName} {contactDto.Email}");
    }
}

static async Task<List<ContactDto>> GetContacts()
{
    var httpClient = new HttpClient();

    // our API requires Accept: application/json
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var response = await httpClient.GetAsync("https://localhost:5001/api/contacts");

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

//static Task GetContact(int id)
//{
//    // TODO:
//}

//static void CreateContact(ContactForCreationDto contact)
//{
//    // TODO:
//}

//static void UpdateContact(int id, ContactForUpdateDto contact)
//{
//    // TODO:
//}

//static void DeleteContact(int id)
//{
//    // TODO:
//}