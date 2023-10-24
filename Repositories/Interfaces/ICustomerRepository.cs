using BusinessObjects;

namespace Repositories.Interfaces;

public interface ICustomerRepository
{
    Task SaveCustomerAsync(Customer p);
    Task UpdateCustomerAsync(Customer p);
    Task DeleteCustomerAsync(Customer p);
    Task<Customer?> GetCustomerAsync(string email);
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<List<Customer>> GetCustomersAsync();    
    Task<List<Customer>> SearchCustomersByNameAsync(string key);    
    bool AdminLogin(string email, string password);
    Task<bool> CustomerLoginAsync(string email, string password);
}
