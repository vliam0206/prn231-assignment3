using BusinessObjects;
using BusinessObjects.DTOs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarRentingWebClient.AccessAPIs;

public class RentingDetailAPIs : IRentingDetailAPIs
{
    private readonly HttpClient _client;
    private readonly string _rentingDetailApiUrl = "";
    private JsonSerializerOptions options;
    public RentingDetailAPIs(IHttpContextAccessor httpContext)
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);

        var token = httpContext.HttpContext!.Session.GetString("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _rentingDetailApiUrl = "https://localhost:7248/api/RentingDetails/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task CreateRentingDetailAsync(List<RentingDetailCreateDTO> rentingDTOs)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(rentingDTOs);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_rentingDetailApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }
    public async Task<List<RentingDetail>> GetRentingDetailsAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_rentingDetailApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var RentingDetails = JsonSerializer.Deserialize<List<RentingDetail>>(strData, options);
        return RentingDetails!;
    }

    public async Task<List<RentingDetail>> GetRentingDetailsByTransactionAsync(int transactionId)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_rentingDetailApiUrl + transactionId);
        string strData = await response.Content.ReadAsStringAsync();
        var RentingDetails = JsonSerializer.Deserialize<List<RentingDetail>>(strData, options);
        return RentingDetails!;
    }
}
