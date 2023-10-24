using BusinessObjects;
using BusinessObjects.DTOs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CarRentingWebClient.AccessAPIs;

public class CustomerAPIs : ICustomerAPIs
{
    private readonly HttpClient _client;
    private readonly string _customerApiUrl = "";
    private JsonSerializerOptions options;
    public CustomerAPIs(IHttpContextAccessor httpContext)
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);

        var token = httpContext.HttpContext!.Session.GetString("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _customerApiUrl = "https://localhost:7248/api/Customers/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task CreateCustomerAsync(CustomerCreateDTO customerDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(customerDTO);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_customerApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }

    public async Task DeleteCustomerAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.DeleteAsync(_customerApiUrl + id);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_customerApiUrl + id);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        else if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
        string strData = await response.Content.ReadAsStringAsync();
        var customer = JsonSerializer.Deserialize<Customer>(strData, options);
        return customer!;
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_customerApiUrl + "get-by-email/" + email);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        else if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
        string strData = await response.Content.ReadAsStringAsync();
        var customer = JsonSerializer.Deserialize<Customer>(strData, options);
        return customer!;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_customerApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var customers = JsonSerializer.Deserialize<List<Customer>>(strData, options);
        return customers!;
    }

    public async Task<List<Customer>> SearchCustomersAsync(string keyword)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_customerApiUrl + $"search/{keyword}");
        string strData = await response.Content.ReadAsStringAsync();
        var customers = JsonSerializer.Deserialize<List<Customer>>(strData, options);
        return customers == null ? new List<Customer>() : customers;
    }

    public async Task UpdateCustomerAsync(int id, CustomerCreateDTO customerDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(customerDTO);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PutAsync(_customerApiUrl + id, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }
}
