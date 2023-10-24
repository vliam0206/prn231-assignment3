using BusinessObjects;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories.Repos;

public class RentingRepository : IRentingRepository
{
    public async Task DeleteRentingTransactionAsync(RentingTransaction p) 
        => await RentingTransactionDAO.DeleteRentingTransactionAsync(p);
    public async Task<RentingTransaction?> GetRentingTransactionByIdAsync(int id) 
        => await RentingTransactionDAO.FindRentingTransactionByIdAsync(id);
    public async Task<List<RentingTransaction>> GetRentingTransactionsAsync() => await RentingTransactionDAO.GetAllTransactionsAsync();
    public async Task SaveRentingTransactionAsync(RentingTransaction p) => await RentingTransactionDAO.SaveEntityAsync(p);
    public async Task UpdateRentingTransactionAsync(RentingTransaction p) => await RentingTransactionDAO.UpdateEntityAsync(p);
    public async Task<List<RentingTransaction>> GetRentingTransactionsAsync(int customerId)
        => await RentingTransactionDAO.GetTransactionsByCustomerAsync(customerId);

    public async Task<List<RentingTransaction>> GetRentingTransactionsByDateAsync(DateTime startDate, DateTime endDate)
    {
        return (await RentingTransactionDAO.GetAllTransactionsAsync())
                    .Where(x => x.RentingDate >= startDate && x.RentingDate <= endDate)
                    .OrderByDescending(x => x.RentingDate)
                    .ToList();
    }
}
