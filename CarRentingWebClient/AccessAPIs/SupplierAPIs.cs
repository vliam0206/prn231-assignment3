﻿using BusinessObjects;
using CarRentingWebClient.AccessAPIs.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarRentingWebClient.AccessAPIs;

public class SupplierAPIs : ISupplierAPIs
{
    private readonly HttpClient _client;
    private readonly string _supplierApiUrl = "";
    private JsonSerializerOptions options;
    public SupplierAPIs(IHttpContextAccessor httpContext)
    {
        _client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        _client.DefaultRequestHeaders.Accept.Add(contentType);

        var token = httpContext.HttpContext!.Session.GetString("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _supplierApiUrl = "https://localhost:7248/api/Suppliers/";
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        // Get Response return
        HttpResponseMessage response = await _client.GetAsync(_supplierApiUrl);
        string strData = await response.Content.ReadAsStringAsync();
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(strData, options);
        return suppliers!;
    }
}
