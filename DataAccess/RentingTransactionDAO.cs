using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class RentingTransactionDAO : BaseDAO<RentingTransaction>
{
    public static async Task<RentingTransaction?> FindRentingTransactionByIdAsync(int RentingTransationId)
    {
        var RentingTransaction = new RentingTransaction();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                RentingTransaction = await context.RentingTransactions
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync(x => x.RentingTransationId == RentingTransationId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return RentingTransaction;
    }
    public static async Task DeleteRentingTransactionAsync(RentingTransaction p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var RentingTransaction = await context.RentingTransactions.FirstOrDefaultAsync(x => x.RentingTransationId == p.RentingTransationId);
                if (RentingTransaction != null)
                {
                    context.RentingTransactions.Remove(RentingTransaction);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task<List<RentingTransaction>> GetAllTransactionsAsync()
    {
        var RentingTransactions = new List<RentingTransaction>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                RentingTransactions = await context.RentingTransactions
                    .Include(x => x.Customer)
                    .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return RentingTransactions;
    }
    public static async Task<List<RentingTransaction>> GetTransactionsByCustomerAsync(int customerId)
    {
        var RentingTransactions = new List<RentingTransaction>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                RentingTransactions = await context.RentingTransactions
                    .Where(x => x.CustomerId == customerId)
                    .Include(x => x.RentingDetails)
                    .Include(x => x.Customer)
                    .ToListAsync();
            }
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return RentingTransactions;
    }
}
