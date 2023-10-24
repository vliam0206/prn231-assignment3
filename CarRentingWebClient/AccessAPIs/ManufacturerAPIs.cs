using BusinessObjects;
using CarRentingWebClient.AccessAPIs.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarRentingWebClient.AccessAPIs;

public class ManufacturerAPIs : IManufacturerAPIs
{
    private readonly HttpClient _client;
    private readonly string _manufacturerApiUrl = "";
    private JsonSerializerOptions options;
    public ManufacturerAPIs(IHttpContextAccessor httpContext)
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);

        var token = httpContext.HttpContext!.Session.GetString("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _manufacturerApiUrl = "https://localhost:7248/api/Manufacturers/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<List<Manufacturer>> GetManufacturersAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_manufacturerApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(strData, options);
        return manufacturers!;
    }
}
