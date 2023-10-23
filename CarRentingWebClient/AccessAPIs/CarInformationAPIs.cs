using BusinessObjects.DTOs;
using BusinessObjects;
using CarRentingWebClient.AccessAPIs.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CarRentingWebClient.Models;

namespace CarRentingWebClient.AccessAPIs;

public class CarInformationAPIs : ICarInformationAPIs
{
    private readonly HttpClient _client;
    private readonly string _carApiUrl = "";
    private JsonSerializerOptions options;
    public CarInformationAPIs()
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);
        _carApiUrl = "https://localhost:7248/api/CarInformations/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task CreateCarInformationAsync(CarCreateDTO carDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(carDTO);
        // Create a StringContent object with JSON data
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_carApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }

    public async Task DeleteCarInformationAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.DeleteAsync(_carApiUrl + id);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode}: {errorMessage}");
        }
    }

    public async Task<List<CarInformation>> GetAvailableCarsAsync(RentingDate rentingDate)
    {
        var jsonStr = JsonSerializer.Serialize(rentingDate);
        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        // Get Response return
        HttpResponseMessage response = await _client.PostAsync(_carApiUrl + "available", content);
        string strData = await response.Content.ReadAsStringAsync();
        var CarInformations = JsonSerializer.Deserialize<List<CarInformation>>(strData, options);
        return CarInformations!;
    }

    public async Task<CarInformation?> GetCarInformationAsync(int id)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl + id);
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
        var CarInformation = JsonSerializer.Deserialize<CarInformation>(strData, options);
        return CarInformation!;
    }

    public async Task<List<CarInformation>> GetCarInformationsAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var CarInformations = JsonSerializer.Deserialize<List<CarInformation>>(strData, options);
        return CarInformations!;
    }

    public async Task<List<CarInformation>> GetValidCarInformationsAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl+"valid");
        string strData = await response.Content.ReadAsStringAsync();
        var CarInformations = JsonSerializer.Deserialize<List<CarInformation>>(strData, options);
        return CarInformations!;
    }

    public async Task<List<CarInformation>> SearchCarsAsync(string keyword)
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_carApiUrl + $"search/{keyword}");
        string strData = await response.Content.ReadAsStringAsync();
        var CarInformations = JsonSerializer.Deserialize<List<CarInformation>>(strData, options);
        return CarInformations ==  null ? new List<CarInformation>() : CarInformations;
    }

    public async Task UpdateCarInformationAsync(int id, CarCreateDTO carDTO)
    {
        // Serialize the product object to JSON
        var jsonString = JsonSerializer.Serialize(carDTO);
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
