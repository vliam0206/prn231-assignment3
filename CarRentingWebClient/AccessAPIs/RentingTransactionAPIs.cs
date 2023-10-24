using BusinessObjects.DTOs;
using BusinessObjects;
using CarRentingWebClient.AccessAPIs.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CarRentingWebClient.AccessAPIs;

public class RentingTransactionAPIs : IRentingTransactionAPIs
{
    private readonly HttpClient _client;
    private readonly string _carApiUrl = "";
    private JsonSerializerOptions options;
    public RentingTransactionAPIs(IHttpContextAccessor httpContext)
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);

        var token = httpContext.HttpContext!.Session.GetString("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _carApiUrl = "https://localhost:7248/api/RentingTransactions/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<RentingTransaction> CreateRentingTransactionAsync(RentingCreateDTO rentingDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(rentingDTO);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_carApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }

        string strData = await response.Content.ReadAsStringAsync();
        var RentingTransaction = JsonSerializer.Deserialize<RentingTransaction>(strData, options);
        return RentingTransaction!;
    }

    public async Task DeleteRentingTransactionAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.DeleteAsync(_carApiUrl + id);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }

    public async Task<RentingTransaction?> GetRentingTransactionAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl + id);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        } else if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
        string strData = await response.Content.ReadAsStringAsync();
        var RentingTransaction = JsonSerializer.Deserialize<RentingTransaction>(strData, options);
        return RentingTransaction!;
    }

    public async Task<List<RentingTransaction>> GetRentingTransactionByCustomerAsync(int customerId)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl + "get-by-customer/" + customerId);
        string strData = await response.Content.ReadAsStringAsync();
        var RentingTransactions = JsonSerializer.Deserialize<List<RentingTransaction>>(strData, options);
        return RentingTransactions!;
    }

    public async Task<List<RentingTransaction>> GetRentingTransactionsAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var RentingTransactions = JsonSerializer.Deserialize<List<RentingTransaction>>(strData, options);
        return RentingTransactions!;
    }

    public async Task UpdateRentingTransactionAsync(int id, RentingCreateDTO rentingDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(rentingDTO);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PutAsync(_carApiUrl + id, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }
}
