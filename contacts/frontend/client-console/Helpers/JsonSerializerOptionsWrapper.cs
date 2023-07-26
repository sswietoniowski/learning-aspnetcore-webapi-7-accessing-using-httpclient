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