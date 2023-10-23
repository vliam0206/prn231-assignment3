using BusinessObjects.DTOs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarRentingWebClient.AccessAPIs;

public class AuthAPIs : IAuthAPIs
{
    private readonly HttpClient _client;
    private readonly string _authApiUrl = "";
    public AuthAPIs()
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);
        _authApiUrl = "https://localhost:7248/api/Auth/";
    }
    public async Task<bool> CustomerLoginAsync(LoginDTO loginDto)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(loginDto);

        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_authApiUrl + "customer-login", content);
        string strData = await response.Content.ReadAsStringAsync();
        return bool.Parse(strData);
    }
    public async Task<bool> AdminLoginAsync(LoginDTO loginDto)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(loginDto);

        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_authApiUrl + "admin-login", content);
        string strData = await response.Content.ReadAsStringAsync();
        return bool.Parse(strData);
    }
}
