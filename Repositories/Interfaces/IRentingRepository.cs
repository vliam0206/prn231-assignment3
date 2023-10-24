using BusinessObjects;

namespace Repositories.Interfaces;

public interface IRentingRepository
{
    Task SaveRentingTransactionAsync(RentingTransaction p);
    Task UpdateRentingTransactionAsync(RentingTransaction p);
    Task DeleteRentingTransactionAsync(RentingTransaction p);
    Task<RentingTransaction?> GetRentingTransactionByIdAsync(int id);
    Task<List<RentingTransaction>> GetRentingTransactionsAsync();    

    Task<List<RentingTransaction>> GetRentingTransactionsAsync(int customerId);
    Task<List<RentingTransaction>> GetRentingTransactionsByDateAsync(DateTime startDate, DateTime endDate);
}
