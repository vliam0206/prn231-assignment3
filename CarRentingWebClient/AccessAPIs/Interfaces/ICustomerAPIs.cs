using BusinessObjects;
using BusinessObjects.DTOs;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface ICustomerAPIs
{
    public Task<List<Customer>> GetCustomersAsync();
    public Task<List<Customer>> SearchCustomersAsync(string keyword);
    public Task<Customer?> GetCustomerAsync(int id);
    public Task<Customer?> GetCustomerByEmailAsync(string email);
    public Task UpdateCustomerAsync(int id, CustomerCreateDTO customerDTO);
    public Task CreateCustomerAsync(CustomerCreateDTO customerDTO);
    public Task DeleteCustomerAsync(int id);
}
