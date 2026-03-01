using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Lar.TesteDotNet.Application.Pagination;

public sealed record Cursor(DateTime? Date, long? Id)
{
    public static string Encode(DateTime? date, long? id)
    {
        var cursor = new Cursor(date, id);
        var json = JsonSerializer.Serialize(cursor);
        return Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(json));
    }

    public static Cursor? Decode(string cursor)
    {
        if (string.IsNullOrEmpty(cursor)) return null;

        try
        {
            var json = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(cursor));
            return JsonSerializer.Deserialize<Cursor>(json);
        }
        catch
        {
            return null;
        }
    }
}