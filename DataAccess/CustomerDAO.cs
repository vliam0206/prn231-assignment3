using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class CustomerDAO : BaseDAO<Customer>
{
    public static async Task<Customer?> FindCustomerByIdAsync(int CustomerId)
    {
        var Customer = new Customer();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                Customer = await context.Customers.FindAsync(CustomerId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Customer;
    }
    public static async Task<Customer?> FindCustomerByEmailAsync(string email)
    {
        var Customer = new Customer();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                Customer = await context.Customers.FirstOrDefaultAsync(x => x.Email.Equals(email));
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Customer;
    }
    public static async Task DeleteCustomerAsync(Customer p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var Customer = await context.Customers.FindAsync(p.CustomerId);
                if (Customer != null)
                {
                    context.Customers.Remove(Customer);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task<List<Customer>> FindCustomersByNameAsync(string key)
    {
        var customers = new List<Customer>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                customers = await context.Customers
                                            .Where(x => x.CustomerName!.Contains(key))
                                            .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return customers;
    }
}
