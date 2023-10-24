using BusinessObjects;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;

namespace Repositories.Repos;

public class CustomerRepository : ICustomerRepository
{
    public async Task DeleteCustomerAsync(Customer p) => await CustomerDAO.DeleteCustomerAsync(p);

    public async Task<List<Customer>> GetCustomersAsync() => await CustomerDAO.GetEntitiesAsync();
    public async Task<Customer?> GetCustomerAsync(string email)
        => await CustomerDAO.FindCustomerByEmailAsync(email);
    public async Task<Customer?> GetCustomerByIdAsync(int id)
        => await CustomerDAO.FindCustomerByIdAsync(id);
    public async Task SaveCustomerAsync(Customer p) => await CustomerDAO.SaveEntityAsync(p);

    public async Task UpdateCustomerAsync(Customer p) => await CustomerDAO.UpdateEntityAsync(p);

    public bool AdminLogin(string email, string password)
    {
        var adminAcc = GetAccountFromJson();
        if (adminAcc.Email == email && adminAcc.Password == password)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> CustomerLoginAsync(string email, string password)
    {
        var customer = await CustomerDAO.FindCustomerByEmailAsync(email);
        if (customer != null && customer.Password == password)
        {
            return true;
        }
        return false;
    }

    private Account GetAccountFromJson()
    {
        IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        string email = config["AdminAccount:Email"];
        string password = config["AdminAccount:Password"];
        Account acc = new Account
        {
            Email = email,
            Password = password
        };

        return acc;
    }

    public async Task<List<Customer>> SearchCustomersByNameAsync(string key)
        => await CustomerDAO.FindCustomersByNameAsync(key);

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await CustomerDAO.FindCustomerByEmailAsync(email);
    }
}

public class Account
{
    public string Email { get; set; }
    public string Password { get; set; }
}
