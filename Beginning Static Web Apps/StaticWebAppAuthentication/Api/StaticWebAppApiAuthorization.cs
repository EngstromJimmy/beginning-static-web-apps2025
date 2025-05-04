using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using StaticWebAppAuthentication.Models;

namespace StaticWebAppAuthentication.Api;
public static class StaticWebAppApiAuthorization
{
    public static bool TryParseHttpHeaderForClientPrincipal(
        HttpHeadersCollection headers,
        out ClientPrincipal? clientPrincipal)
    {
        if (!headers.Contains("x-ms-client-principal"))
        {
            clientPrincipal = null;
            return false;
        }

        try
        {
            var data = headers.FirstOrDefault(header => header.Key.ToLower() == "x-ms-client-principal");
            var decoded = Convert.FromBase64String(data.Value.First());
            var json = Encoding.UTF8.GetString(decoded);

            clientPrincipal =
                JsonSerializer.Deserialize<ClientPrincipal>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return clientPrincipal is not null;
        }
        catch
        {
            clientPrincipal = null;
            return false;
        }
    }
}